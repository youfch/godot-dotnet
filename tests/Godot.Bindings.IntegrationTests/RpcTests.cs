using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Bindings.IntegrationTests;

public sealed class RpcTests : IntegrationTestBase
{
    public RpcTests(GodotBinaryFixture godot, ITestOutputHelper output) : base(godot, output) { }

    [Fact]
    public Task Rpc()
    {
        return Verify("res://TestRpc.tscn");
    }
}
