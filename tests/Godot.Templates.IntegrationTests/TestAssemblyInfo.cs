using System;
using System.Reflection;
using System.Runtime.Versioning;
using NuGet.Frameworks;

namespace Godot.Templates.IntegrationTests;

internal static class TestAssemblyInfo
{
    /// <summary>
    /// The version of the Godot .NET packages that the test assembly was built against.
    /// Should match the version of the template packages.
    /// </summary>
    public static string GodotVersion { get; }

    /// <summary>
    /// The target framework that the test assembly was built against, in short folder name format (e.g. "net9.0").
    /// Should match the TFM used by the template packages.
    /// </summary>
    public static string TargetFramework { get; }

    static TestAssemblyInfo()
    {
        var assembly = typeof(IntegrationTestBase).Assembly;
        var assemblyInformationalVersionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        string? informationalVersion = assemblyInformationalVersionAttribute?.InformationalVersion;
        if (string.IsNullOrEmpty(informationalVersion))
        {
            throw new InvalidOperationException("Could not determine the version of the Godot .NET packages.");
        }

        var targetFrameworkAttribute = assembly.GetCustomAttribute<TargetFrameworkAttribute>();
        string? frameworkName = targetFrameworkAttribute?.FrameworkName;
        if (string.IsNullOrEmpty(frameworkName))
        {
            throw new InvalidOperationException("Could not determine the TFM of the test assembly.");
        }

        GodotVersion = informationalVersion;
        TargetFramework = NuGetFramework.Parse(frameworkName).GetShortFolderName();
    }
}
