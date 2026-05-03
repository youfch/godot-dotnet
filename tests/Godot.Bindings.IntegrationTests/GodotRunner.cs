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

    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);

    public required string GodotBinary { get; init; }

    public ImmutableArray<string> GodotArgs { get; set; } = [];

    public required string ProjectDir { get; init; }

    public ITestOutputHelper? Output { get; init; }

    public TimeSpan Timeout { get; set; } = DefaultTimeout;

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
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(Timeout);

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
            return parsedResults;
        }

        return [new GodotTestResult(false, $"Godot process exited with code {process.ExitCode} without printing a GODOT_TEST_RESULT line with args [{string.Join(", ", process.StartInfo.ArgumentList)}].")];
    }
}
