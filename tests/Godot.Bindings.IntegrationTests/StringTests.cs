using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Bindings.IntegrationTests;

public sealed class StringTests : IntegrationTestBase
{
    public StringTests(GodotBinaryFixture godot, ITestOutputHelper output) : base(godot, output) { }

    [Fact]
    public Task StringMarshalling()
    {
        return Verify("res://TestStrings.tscn");
    }
}
