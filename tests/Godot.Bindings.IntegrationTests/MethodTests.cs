using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Bindings.IntegrationTests;

public sealed class MethodTests : IntegrationTestBase
{
    public MethodTests(GodotBinaryFixture godot, ITestOutputHelper output) : base(godot, output) { }

    [Fact]
    public Task Methods()
    {
        return Verify("res://TestMethods.tscn");
    }

    [Fact]
    public Task MethodsWithDefaultValues()
    {
        return Verify("res://TestMethodsWithDefaultValues.tscn");
    }

    [Fact]
    public Task StaticMethods()
    {
        return Verify("res://TestStaticMethods.tscn");
    }

    [Fact]
    public Task VirtualMethods()
    {
        return Verify("res://TestVirtualMethods.tscn");
    }

    [Fact]
    public Task MethodsFromCSharp()
    {
        return Verify("res://TestMethodsFromCSharp.tscn");
    }
}
