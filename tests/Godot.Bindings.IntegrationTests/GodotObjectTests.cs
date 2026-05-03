using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Bindings.IntegrationTests;

public sealed class GodotObjectTests : IntegrationTestBase
{
    public GodotObjectTests(GodotBinaryFixture godot, ITestOutputHelper output) : base(godot, output) { }

    [Fact]
    public Task GodotObjectToString()
    {
        return Verify("res://TestGodotObjectToString.tscn");
    }

    [Fact]
    public Task GodotObjectConstruction()
    {
        return Verify("res://TestGodotObjectConstruction.tscn");
    }

    [Fact]
    public Task GodotObjectCast()
    {
        return Verify("res://TestGodotObjectCast.tscn");
    }

    [Fact]
    public Task GodotObjectUnicodeName()
    {
        return Verify("res://TestGodotObjectUnicodeName.tscn");
    }

    [Fact]
    public Task GodotObjectDisposal()
    {
        return Verify("res://TestGodotObjectDisposal.tscn");
    }

    [Fact(Skip = "InstanceIsValid does not work, it's fixed in the WIP branch where we use a new construction/destruction implementation using the GDExtension callbacks.")]
    public Task GodotObjectInstanceIsValid()
    {
        return Verify("res://TestGodotObjectInstanceIsValid.tscn");
    }
}
