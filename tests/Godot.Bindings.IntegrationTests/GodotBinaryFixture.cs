using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NuGet.Versioning;

namespace Godot.Bindings.IntegrationTests;

public sealed class GodotBinaryFixture : IAsyncLifetime
{
    private const string GodotEnvironmentVariable = "GODOT_PATH";
    private const string GodotCacheDirName = ".godot";
    private const string GodotVersionFileName = "version.txt";
    private const string RequiredVersion = "4.7-dotnet4";
    // TODO: This hardcodes the download URL to the GitHub prereleases, switch to an official distribution method once available.
    private const string DownloadBaseUrl = $"https://github.com/raulsntos/godot/releases/download/{RequiredVersion}/";

    public string GodotBinaryPath { get; private set; } = string.Empty;

    public string TestGamePath { get; } = TestPaths.TestGamePath;

    public async Task InitializeAsync()
    {
        if (TryGetGodotFromEnvironmentVariable(out string? binaryPath))
        {
            GodotBinaryPath = binaryPath;
            return;
        }

        if (TryGetGodotFromCacheDirectory(out binaryPath))
        {
            GodotBinaryPath = binaryPath;
            return;
        }

        await Task.WhenAll([
            DownloadGodotToCacheDirectory(),
            DownloadPackagesToCacheDirectory(),
        ]);

        // Try to lookup the binary in the cache again after trying to download it, in case the download succeeded.
        if (TryGetGodotFromCacheDirectory(out binaryPath))
        {
            GodotBinaryPath = binaryPath;

            // If we have just downloaded a new binary, we need to let it fetch the 'Godot.EditorIntegration' package.
            // We assume that if the binary was found before or it's a custom one provided by the environment variable,
            // it has already fetched the package.
            SetupPackages(GodotBinaryPath);

            return;
        }

        throw new InvalidOperationException("Godot binary not found and could not be downloaded.");
    }

    private static bool TryGetGodotFromEnvironmentVariable([NotNullWhen(true)] out string? path)
    {
        path = Environment.GetEnvironmentVariable(GodotEnvironmentVariable);
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        if (!Path.Exists(path))
        {
            throw new FileNotFoundException($"{GodotEnvironmentVariable} points to a non-existent file: {path}", path);
        }

        if (OperatingSystem.IsMacOS() && Path.GetExtension(path) == ".app")
        {
            // If the provided path looks like an .app bundle, we'll return the path
            // to the executable inside the bundle, since that's what we need to run.
            path = Path.Combine(path, "Contents", "MacOS", "Godot");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"{GodotEnvironmentVariable} points to an .app bundle that does not contain the expected executable: {path}", path);
            }
        }

        return true;
    }

    private static bool TryGetGodotFromCacheDirectory([NotNullWhen(true)] out string? path)
    {
        string cacheDir = Path.Combine(TestPaths.RepoRoot, GodotCacheDirName);
        if (!Directory.Exists(cacheDir))
        {
            path = null;
            return false;
        }

        if (!TryGetGodotBinaryName(out string? godotBinaryName))
        {
            throw new PlatformNotSupportedException("Running Godot binary on unsupported platform.");
        }

        path = Path.Combine(cacheDir, godotBinaryName);
        if (!Path.Exists(path))
        {
            path = null;
            return false;
        }

        if (OperatingSystem.IsMacOS() && Path.GetExtension(path) == ".app")
        {
            // If the provided path looks like an .app bundle, we'll return the path
            // to the executable inside the bundle, since that's what we need to run.
            path = Path.Combine(path, "Contents", "MacOS", "Godot");
            if (!File.Exists(path))
            {
                return false;
            }
        }

        try
        {
            string versionFilePath = Path.Combine(cacheDir, GodotVersionFileName);
            string versionString = File.ReadAllText(versionFilePath).Trim();
            var version = NuGetVersion.Parse(versionString);
            // TODO: Newer versions may be compatible, we should consider '<' instead of exact match, but we're currently downloading preview releases which don't strictly follow semver.
            if (version != NuGetVersion.Parse(RequiredVersion))
            {
                // The cached binary is not compatible.
                path = null;
                return false;
            }
        }
        catch (Exception)
        {
            // The version file is missing or invalid.
            // We can't check if the binary version is compatible,
            // so we treat it as if the binary was not found.
            path = null;
            return false;
        }

        return true;
    }

    private static bool TryGetGodotBinaryName([NotNullWhen(true)] out string? binaryName)
    {
        if (OperatingSystem.IsLinux())
        {
            binaryName = "godot";
            return true;
        }
        else if (OperatingSystem.IsWindows())
        {
            binaryName = "godot.exe";
            return true;
        }
        else if (OperatingSystem.IsMacOS())
        {
            binaryName = "Godot.app";
            return true;
        }

        binaryName = null;
        return false;
    }

    private static bool TryGetGodotBinaryDownloadUrl([NotNullWhen(true)] out string? binaryDownloadUrl, [NotNullWhen(true)] out string? archivedBinaryName)
    {
        if (OperatingSystem.IsLinux())
        {
            (binaryDownloadUrl, archivedBinaryName) = RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X64 => ($"{DownloadBaseUrl}Godot_v{RequiredVersion}_dotnet_linux.x86_64.zip", $"Godot_v{RequiredVersion}_dotnet_linux.x86_64"),
                Architecture.X86 => ($"{DownloadBaseUrl}Godot_v{RequiredVersion}_dotnet_linux.x86_32.zip", $"Godot_v{RequiredVersion}_dotnet_linux.x86_32"),
                Architecture.Arm64 => ($"{DownloadBaseUrl}Godot_v{RequiredVersion}_dotnet_linux.arm64.zip", $"Godot_v{RequiredVersion}_dotnet_linux.arm64"),
                Architecture.Arm => ($"{DownloadBaseUrl}Godot_v{RequiredVersion}_dotnet_linux.arm32.zip", $"Godot_v{RequiredVersion}_dotnet_linux.arm32"),
                _ => default,
            };
        }
        else if (OperatingSystem.IsWindows())
        {
            (binaryDownloadUrl, archivedBinaryName) = RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X64 => ($"{DownloadBaseUrl}Godot_v{RequiredVersion}_dotnet_win64.exe.zip", $"Godot_v{RequiredVersion}_dotnet_win64.exe"),
                Architecture.X86 => ($"{DownloadBaseUrl}Godot_v{RequiredVersion}_dotnet_win32.exe.zip", $"Godot_v{RequiredVersion}_dotnet_win32.exe"),
                Architecture.Arm64 => ($"{DownloadBaseUrl}Godot_v{RequiredVersion}_dotnet_windows_arm64.exe.zip", $"Godot_v{RequiredVersion}_dotnet_windows_arm64.exe"),
                _ => default,
            };
        }
        else if (OperatingSystem.IsMacOS())
        {
            binaryDownloadUrl = $"{DownloadBaseUrl}Godot_v{RequiredVersion}_dotnet_macos.universal.zip";
            archivedBinaryName = "Godot_dotnet.app";
        }
        else
        {
            binaryDownloadUrl = null;
            archivedBinaryName = null;
        }

        return !string.IsNullOrEmpty(binaryDownloadUrl) && !string.IsNullOrEmpty(archivedBinaryName);
    }

    private static async Task DownloadGodotToCacheDirectory()
    {
        if (!TryGetGodotBinaryDownloadUrl(out string? binaryDownloadUrl, out string? archivedBinaryName) || !TryGetGodotBinaryName(out string? godotBinaryName))
        {
            throw new PlatformNotSupportedException("Downloading Godot binary on unsupported platform or architecture.");
        }

        Console.WriteLine($"Downloading Godot binary from {binaryDownloadUrl}");

        int maxRetries = 3;
        while (maxRetries-- > 0)
        {
            try
            {
                string cacheDir = Path.Combine(TestPaths.RepoRoot, GodotCacheDirName);
                Directory.CreateDirectory(cacheDir);

                using HttpClient httpClient = new();
                using var stream = await httpClient.GetStreamAsync(binaryDownloadUrl);
                using var archive = new ZipArchive(stream);

                if (OperatingSystem.IsMacOS())
                {
                    // macOS binaries are distributed as .app bundles which are basically directories,
                    // so we can't get a file entry and extract it directly. Instead, we extract the
                    // whole archive and then rename the .app bundle.
                    archive.ExtractToDirectory(cacheDir, overwriteFiles: true);

                    string archivedBundlePath = Path.Combine(cacheDir, archivedBinaryName);
                    string godotBundlePath = Path.Combine(cacheDir, godotBinaryName);

                    if (Directory.Exists(godotBundlePath))
                    {
                        Directory.Delete(godotBundlePath, recursive: true);
                    }

                    Directory.Move(archivedBundlePath, godotBundlePath);
                }
                else
                {
                    var entry = archive.GetEntry(archivedBinaryName);
                    if (entry is null)
                    {
                        // No need to retry since the archive is not in the expected format.
                        maxRetries = 0;

                        throw new InvalidOperationException($"The downloaded archive does not contain the expected binary '{archivedBinaryName}'.");
                    }

                    entry.ExtractToFile(Path.Combine(cacheDir, godotBinaryName), overwrite: true);
                }

                File.WriteAllText(Path.Combine(cacheDir, GodotVersionFileName), RequiredVersion);

                return;
            }
            catch (Exception)
            {
                // Unable to download or extract the binary. We try again until maxRetries is exhausted.
                // If we still fail, we'll end up throwing later since the binary won't be found.
                Console.WriteLine($"Failed to download or extract the Godot binary. Retrying... ({3 - maxRetries}/3)");
            }
        }

        throw new InvalidOperationException($"Failed to download the Godot binary from {binaryDownloadUrl} after multiple attempts.");
    }

    private static async Task DownloadPackagesToCacheDirectory()
    {
        string packagesDownloadUrl = $"{DownloadBaseUrl}godot-dotnet-{RequiredVersion.Replace('-', '.')}.zip";

        Console.WriteLine($"Downloading Godot .NET packages from {packagesDownloadUrl}");

        int maxRetries = 3;
        while (maxRetries-- > 0)
        {
            try
            {
                string cacheDir = Path.Combine(TestPaths.RepoRoot, GodotCacheDirName, "packages");
                Directory.CreateDirectory(cacheDir);

                using HttpClient httpClient = new();
                using var stream = await httpClient.GetStreamAsync(packagesDownloadUrl);
                using var archive = new ZipArchive(stream);
                archive.ExtractToDirectory(cacheDir, overwriteFiles: true);

                return;
            }
            catch (Exception)
            {
                // Unable to download or extract the packages. We try again until maxRetries is exhausted.
                // If we still fail, we'll end up throwing later since the packages won't be found.
                Console.WriteLine($"Failed to download or extract the Godot .NET packages. Retrying... ({3 - maxRetries}/3)");
            }
        }

        throw new InvalidOperationException($"Failed to download the Godot .NET packages from {packagesDownloadUrl} after multiple attempts.");
    }

    private static void SetupPackages(string godotBinaryPath)
    {
        // If the packages were downloaded, add them to the NuGet configuration
        // so they can be used to fetch the 'Godot.EditorIntegration'.
        string packagesDir = Path.Combine(TestPaths.RepoRoot, GodotCacheDirName, "packages");
        if (Directory.Exists(packagesDir))
        {
            string nugetConfigPath = Path.Combine(TestPaths.RepoRoot, GodotCacheDirName, "NuGet.config");
            if (OperatingSystem.IsMacOS())
            {
                // On macOS, since the Godot binary can't write to the .app bundle,
                // the packages need to be placed outside of it.
                string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var version = NuGetVersion.Parse(RequiredVersion);
                string versionFolderName = $"{version.Major}.{version.Minor}.{version.Patch}-{version.Release}";
                string godotDir = Path.Combine(home, "Library", "Application Support", "Godot", "Godot.NET", versionFolderName);
                Directory.CreateDirectory(godotDir);
                nugetConfigPath = Path.Combine(godotDir, "NuGet.config");
                Console.WriteLine($"Writing NuGet.config to {nugetConfigPath}");
            }

            File.WriteAllText(nugetConfigPath, $"""
                <configuration>
                  <packageSources>
                    <clear />
                    <add key="nuget" value="https://api.nuget.org/v3/index.json" />
                    <add key="CachedPackages" value="{packagesDir}" />
                  </packageSources>
                  <packageSourceMapping>
                    <packageSource key="nuget">
                      <package pattern="*" />
                    </packageSource>
                    <packageSource key="CachedPackages">
                      <package pattern="Godot" />
                      <package pattern="Godot.*" />
                    </packageSource>
                  </packageSourceMapping>
                </configuration>
                """);
        }

        using var process = Process.Start(godotBinaryPath, ["--enable-dotnet-features"]);

        process.WaitForExit();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
