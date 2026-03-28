using System;
using System.IO;

namespace Godot.Templates.IntegrationTests;

internal static class TestPaths
{
    public static string RepoRoot { get; } = FindRepoRoot();

    public static string PackagesPath { get; } =
#if RELEASE
        Path.Combine(RepoRoot, "artifacts", "packages", "Release", "Shipping");
#else
        Path.Combine(RepoRoot, "artifacts", "packages", "Debug", "Shipping");
#endif

    public static string TemplatesPackagePath { get; } =
        Path.Combine(PackagesPath, $"Godot.Templates.{TestAssemblyInfo.GodotVersion}.nupkg");

    private static string FindRepoRoot()
    {
        DirectoryInfo? current = new(AppContext.BaseDirectory);
        while (current is not null && !File.Exists(Path.Combine(current.FullName, "global.json")))
        {
            current = current.Parent;
        }

        if (string.IsNullOrEmpty(current?.FullName))
        {
            throw new InvalidOperationException($"Could not find the repository root. Expected a directory containing 'global.json' in the path: {AppContext.BaseDirectory}");
        }

        return current.FullName;
    }
}
