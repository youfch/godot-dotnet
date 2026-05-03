using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Bindings.IntegrationTests;

public sealed class VariantTests : IntegrationTestBase
{
    public VariantTests(GodotBinaryFixture godot, ITestOutputHelper output) : base(godot, output) { }

    [Fact]
    public Task Variant()
    {
        return Verify("res://TestVariant.tscn");
    }
}
