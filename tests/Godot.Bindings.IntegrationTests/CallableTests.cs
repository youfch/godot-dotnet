using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Bindings.IntegrationTests;

public sealed class CallableTests : IntegrationTestBase
{
    public CallableTests(GodotBinaryFixture godot, ITestOutputHelper output) : base(godot, output) { }

    [Fact]
    public Task Callables()
    {
        return Verify("res://TestCallables.tscn");
    }
}
