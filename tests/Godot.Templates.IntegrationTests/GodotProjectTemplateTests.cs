using System.IO;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Templates.IntegrationTests;

[Collection(nameof(TemplatesCollectionFixture))]
public sealed class GodotProjectTemplateTests : IntegrationTestBase
{
    public GodotProjectTemplateTests(TemplatesFixture templates, ITestOutputHelper output) : base(templates, output) { }

    [Fact]
    public async Task DefaultInstantiation()
    {
        var result = await Verify("godot",
            "--name", "TestProject",
            "--godot", TestAssemblyInfo.GodotVersion,
            "--framework", TestAssemblyInfo.TargetFramework,
            "--no-restore",
            "--output", ".");

        string projectGodot = File.ReadAllText(Path.Combine(result.Path, "project.godot"));
        Assert.Contains("config/name=\"TestProject\"", projectGodot);
        Assert.Contains("project/assembly_name=\"TestProject\"", projectGodot);
    }

    [Fact]
    public async Task CustomGodotVersionSubstitutedIntoSdkAttribute()
    {
        var result = await Verify("godot",
            "--name", "TestProject",
            "--godot", "4.8.0",
            "--framework", TestAssemblyInfo.TargetFramework,
            "--no-restore",
            "--output", ".");

        string csproj = File.ReadAllText(Path.Combine(result.Path, "TestProject.csproj"));
        Assert.Contains("Sdk=\"Godot.NET.Sdk/4.8.0\"", csproj);
    }

    [Fact]
    public async Task BuildsSuccessfully()
    {
        var verifier = MakeVerifier();

        verifier.BuildProject = true;

        await verifier.RunAsync("godot",
            "--name", "BuildTest",
            "--no-restore",
            "--output", ".");

        string csproj = File.ReadAllText(Path.Combine(verifier.TemplateOutputDir.FullName, "BuildTest.csproj"));
        Assert.Contains($"Sdk=\"Godot.NET.Sdk/{TestAssemblyInfo.GodotVersion}\"", csproj);
        Assert.Contains($"<TargetFramework>{TestAssemblyInfo.TargetFramework}</TargetFramework>", csproj);
    }
}
