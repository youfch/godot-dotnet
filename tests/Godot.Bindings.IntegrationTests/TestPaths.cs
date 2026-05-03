using System;
using System.IO;

namespace Godot.Bindings.IntegrationTests;

internal static class TestPaths
{
    public static string RepoRoot { get; } = FindRepoRoot();

    public static string TestGamePath { get; } =
        Path.Combine(RepoRoot, "tests", "Godot.Bindings.IntegrationTests.TestGame");

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
