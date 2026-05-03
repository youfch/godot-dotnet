using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Bindings.IntegrationTests;

public sealed class SignalTests : IntegrationTestBase
{
    public SignalTests(GodotBinaryFixture godot, ITestOutputHelper output) : base(godot, output) { }

    [Fact]
    public Task Signal()
    {
        return Verify("res://TestSignals.tscn");
    }
}
