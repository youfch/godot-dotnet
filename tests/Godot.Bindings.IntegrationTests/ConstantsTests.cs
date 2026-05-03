using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Bindings.IntegrationTests;

public sealed class ConstantsTests : IntegrationTestBase
{
    public ConstantsTests(GodotBinaryFixture godot, ITestOutputHelper output) : base(godot, output) { }

    [Fact]
    public Task ConstantsAndEnums()
    {
        return Verify("res://TestConstants.tscn");
    }
}
