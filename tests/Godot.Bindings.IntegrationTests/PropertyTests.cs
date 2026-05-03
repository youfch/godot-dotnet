using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Bindings.IntegrationTests;

public sealed class PropertyTests : IntegrationTestBase
{
    public PropertyTests(GodotBinaryFixture godot, ITestOutputHelper output) : base(godot, output) { }

    [Fact]
    public Task Properties()
    {
        return Verify("res://TestProperty.tscn");
    }
}
