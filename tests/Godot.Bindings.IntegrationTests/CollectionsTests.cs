using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Bindings.IntegrationTests;

public sealed class CollectionsTests : IntegrationTestBase
{
    public CollectionsTests(GodotBinaryFixture godot, ITestOutputHelper output) : base(godot, output) { }

    [Fact]
    public Task Collections()
    {
        return Verify("res://TestCollections.tscn");
    }
}
