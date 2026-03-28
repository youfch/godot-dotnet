using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Templates.IntegrationTests;

[Collection(nameof(TemplatesCollectionFixture))]
public sealed class GodotNodeTemplateTests : IntegrationTestBase
{
    public GodotNodeTemplateTests(TemplatesFixture templates, ITestOutputHelper output) : base(templates, output) { }

    [Fact]
    public async Task DefaultInstantiation()
    {
        // We have to use --force because this template will fail outside of a project context.
        var result = await Verify("godotnode", "--name", "MyNode", "--force");

        string content = await File.ReadAllTextAsync(Path.Combine(result.Path, "MyNode.cs"));
        Assert.Contains("[GodotClass]", content);
        Assert.Contains("namespace MyNamespace", content);
        Assert.Contains("class MyNode : Node", content);
    }

    [Fact]
    public async Task InProjectContextNamespaceResolved()
    {
        var verifier = MakeVerifier();

        // We create a minimal C# project to test the template with.
        await File.WriteAllTextAsync(Path.Combine(verifier.TemplateOutputDir.FullName, "MyGame.csproj"), $"""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>{TestAssemblyInfo.TargetFramework}</TargetFramework>
              </PropertyGroup>
            </Project>
            """);

        using var process = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = "dotnet",
                ArgumentList = { "restore" },
                WorkingDirectory = verifier.TemplateOutputDir.FullName,
            },
        };
        process.Start();

        await process.WaitForExitAsync();
        Assert.Equal(0, process.ExitCode);

        await verifier.RunAsync("godotnode", "--name", "Player");

        string content = await File.ReadAllTextAsync(Path.Combine(verifier.TemplateOutputDir.FullName, "Player.cs"));
        Assert.Contains("[GodotClass]", content);
        Assert.Contains("namespace MyGame", content);
        Assert.DoesNotContain("namespace MyNamespace", content);
        Assert.Contains("class Player : Node", content);
    }
}
