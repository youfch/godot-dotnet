using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Templates.IntegrationTests;

public sealed class TemplatesCustomHive
{
    public string HiveDirectory { get; } = Directory.CreateTempSubdirectory("GodotTemplatesTestHive").FullName;

    private const string StampFileName = ".stamp";

    public async Task EnsureInstalledAsync(ITestOutputHelper? output = null)
    {
        if (File.Exists(Path.Combine(HiveDirectory, StampFileName)))
        {
            // The hive already exists, we assume it's correctly set up.
            return;
        }

        output?.WriteLine($"Creating templates custom hive at '{HiveDirectory}'.");
        Directory.CreateDirectory(HiveDirectory);

        await InstallTemplatesAsync(TestPaths.TemplatesPackagePath, HiveDirectory, output);
    }

    private static async Task InstallTemplatesAsync(string packagePath, string customHivePath, ITestOutputHelper? output = null)
    {
        using var process = new Process()
        {
            StartInfo =
            {
                FileName = "dotnet",
                ArgumentList =
                {
                    "new", "install",
                    "--debug:custom-hive",
                    customHivePath,
                    packagePath,
                },
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            },
        };

        process.OutputDataReceived += (sender, e) =>
        {
            if (string.IsNullOrEmpty(e.Data))
            {
                return;
            }

            output?.WriteLine(e.Data);
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (string.IsNullOrEmpty(e.Data))
            {
                return;
            }

            output?.WriteLine($"[stderr] {e.Data}");
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Failed to install templates from package '{packagePath}'.");
        }

        // Add a timestamp file to the hive to indicate it has been set up.
        // This is used to skip re-installing the templates for subsequent tests that use the same hive.
        File.WriteAllText(Path.Combine(customHivePath, StampFileName), DateTime.UtcNow.ToString("o"));
    }
}
