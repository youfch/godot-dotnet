using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Templates.IntegrationTests;

public class TemplateRunner
{
    public static readonly TimeSpan DefaultCreateTimeout = TimeSpan.FromSeconds(15);
    public static readonly TimeSpan DefaultBuildTimeout = TimeSpan.FromMinutes(3);

    public required TemplatesCustomHive CustomHive { get; init; }

    public ITestOutputHelper? Output { get; init; }

    public TimeSpan CreateTimeout { get; set; } = DefaultCreateTimeout;
    public TimeSpan BuildTimeout { get; set; } = DefaultBuildTimeout;

    public DirectoryInfo TemplateOutputDir { get; } = Directory.CreateTempSubdirectory("TemplateTest");

    public bool BuildProject { get; set; }

    public async Task RunAsync(string[] dotnetNewArgs, CancellationToken cancellationToken = default)
    {
        await CreateTemplateAsync(dotnetNewArgs, cancellationToken);

        if (BuildProject)
        {
            await BuildProjectAsync(cancellationToken);
        }
    }

    private async Task CreateTemplateAsync(string[] dotnetNewArgs, CancellationToken cancellationToken = default)
    {
        await CustomHive.EnsureInstalledAsync(Output);

        using var process = StartDotNetProcess([
            "new", .. dotnetNewArgs,
            "--debug:custom-hive", CustomHive.HiveDirectory,
        ]);

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(CreateTimeout);

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

            throw new TimeoutException($"dotnet new did not exit within the timeout of {CreateTimeout.TotalSeconds} seconds.");
        }

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"dotnet new exited with code {process.ExitCode}.");
        }
    }

    private async Task BuildProjectAsync(CancellationToken cancellationToken = default)
    {
        // Add NuGet configuration pointing to the built packages, so the project can build successfully.
        // TODO: We use the parent directory to include NonShipping packages, since the Godot.NET.Sdk is not being shipped from this repository yet, we're still shipping from the mono module.
        string packagesPath = $"{TestPaths.PackagesPath}/..";
        File.WriteAllText(Path.Combine(TemplateOutputDir.FullName, "NuGet.config"), $"""
            <configuration>
                <packageSources>
                    <clear />
                    <add key="BuiltPackages" value="{packagesPath}" />
                </packageSources>
            </configuration>
            """);

        using var process = StartDotNetProcess(["build"]);

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(BuildTimeout);

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

            throw new TimeoutException($"dotnet build did not exit within the timeout of {BuildTimeout.TotalSeconds} seconds.");
        }

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"dotnet build exited with code {process.ExitCode}.");
        }
    }

    private Process StartDotNetProcess(IEnumerable<string> args)
    {
        var process = new Process()
        {
            StartInfo =
            {
                FileName = "dotnet",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = TemplateOutputDir.FullName,
            },
        };

        foreach (string arg in args)
        {
            process.StartInfo.ArgumentList.Add(arg);
        }

        process.OutputDataReceived += (sender, e) =>
        {
            if (string.IsNullOrEmpty(e.Data))
            {
                return;
            }

            Output?.WriteLine(e.Data);
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (string.IsNullOrEmpty(e.Data))
            {
                return;
            }

            Output?.WriteLine($"[stderr] {e.Data}");
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        return process;
    }
}
