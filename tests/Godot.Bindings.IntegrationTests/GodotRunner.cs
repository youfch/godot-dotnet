using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Bindings.IntegrationTests;

public class GodotRunner
{
    public const string ResultPrefix = "GODOT_TEST_RESULT::";
    public const string ResultPassPrefix = $"{ResultPrefix}PASS:";
    public const string ResultFailPrefix = $"{ResultPrefix}FAIL:";

    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(15);

    public required string GodotBinary { get; init; }

    public ImmutableArray<string> GodotArgs { get; set; } = [];

    public required string ProjectDir { get; init; }

    public ITestOutputHelper? Output { get; init; }

    public TimeSpan Timeout { get; set; } = DefaultTimeout;

    private readonly struct ErrorRule
    {
        public string Prefix { get; init; }
        public string Message { get; init; }
        public bool ExitEarly { get; init; }
    }

    // These are common errors that we could encounter in stderr that indicate a failure we want to catch
    // and convert to a test failure with a clear message, which is more helpful than seeing a crash or
    // a timeout with a generic exit code.
    // Rules that exit early are used for errors related to the test script itself. Since the test script
    // is responsible for exiting the process, we can assume that if we encounter these errors, the process
    // may be stuck and we should kill it to avoid waiting for the timeout.
    private static readonly List<ErrorRule> _errorRules =
    [
        new()
        {
            Prefix = "ERROR: Trying to unreference a SafeRefCount which is already zero",
            Message = "Godot process crashed on unreference. This likely indicates a double-free or similar violation occurred. C# instances may be disposing an already freed native object.",
            ExitEarly = false,
        },
        new()
        {
            Prefix = "ERROR: .NET: Failed to unload assembly 'Godot.Bindings.IntegrationTests.TestGame'",
            Message = "Godot process failed to unload the test assembly. This likely indicates leaked references prevented the ALC unloading.",
            ExitEarly = false,
        },
        new()
        {
            Prefix = "ERROR: Failed to load script",
            Message = "Godot process failed to load a script. There may be a typo in the test script.",
            ExitEarly = true,
        },
        new()
        {
            Prefix = "SCRIPT ERROR:",
            Message = "Godot process printed a script error. There may be an issue in the test script like using a non-existent method or property.",
            ExitEarly = true,
        },
    ];

    public async Task<List<GodotTestResult>> RunAsync(string scenePath, string[] sceneArgs, CancellationToken cancellationToken = default)
    {
        using var process = new Process()
        {
            StartInfo = new ProcessStartInfo(GodotBinary)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            },
        };

        process.StartInfo.ArgumentList.Add("--headless");

        process.StartInfo.ArgumentList.Add("--path");
        process.StartInfo.ArgumentList.Add(ProjectDir);

        process.StartInfo.ArgumentList.Add("--scene");
        process.StartInfo.ArgumentList.Add(scenePath);

        foreach (string arg in GodotArgs)
        {
            process.StartInfo.ArgumentList.Add(arg);
        }

        if (sceneArgs.Length > 0)
        {
            process.StartInfo.ArgumentList.Add("--");

            foreach (string arg in sceneArgs)
            {
                process.StartInfo.ArgumentList.Add(arg);
            }
        }

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(Timeout);

        List<string> resultLines = [];
        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data is null)
            {
                return;
            }

            if (e.Data.StartsWith(ResultPrefix, StringComparison.Ordinal))
            {
                resultLines.Add(e.Data);
                return;
            }

            Output?.WriteLine(e.Data);
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data is null)
            {
                return;
            }

            Output?.WriteLine($"[stderr] {e.Data}");

            foreach (var rule in _errorRules)
            {
                if (e.Data.StartsWith(rule.Prefix, StringComparison.Ordinal))
                {
                    resultLines.Add($"{ResultFailPrefix}{rule.Message}");

                    if (rule.ExitEarly)
                    {
                        // We encountered an issue likely related to the script that is running the test,
                        // since this script is responsible for exiting the process, we need to kill the
                        // process to avoid waiting for the timeout.
                        timeoutCts.Cancel();
                    }

                    break;
                }
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        try
        {
            await process.WaitForExitAsync(timeoutCts.Token);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            // Reached the timeout. Kill the process and return a failure result.
            try
            {
                process.Kill(entireProcessTree: true);
            }
            catch { }

            return [new GodotTestResult(false, $"Godot process did not exit within {Timeout.TotalSeconds}s with args [{string.Join(", ", process.StartInfo.ArgumentList)}].")];
        }

        List<GodotTestResult> parsedResults = [];
        foreach (string line in resultLines)
        {
            var result = GodotTestResult.Parse(line);
            parsedResults.Add(result);
        }

        if (parsedResults.Count > 0)
        {
            if (process.ExitCode != 0)
            {
                parsedResults.Add(new GodotTestResult(false, $"Godot process exited with code {process.ExitCode} with args [{string.Join(", ", process.StartInfo.ArgumentList)}]."));
            }

            return parsedResults;
        }

        return [new GodotTestResult(false, $"Godot process exited with code {process.ExitCode} without printing a GODOT_TEST_RESULT line with args [{string.Join(", ", process.StartInfo.ArgumentList)}].")];
    }
}
