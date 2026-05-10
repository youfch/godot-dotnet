using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Bindings.IntegrationTests;

public sealed class RefCountedTests : IntegrationTestBase
{
    public RefCountedTests(GodotBinaryFixture godot, ITestOutputHelper output) : base(godot, output) { }

    [Fact]
    public Task RefCounted()
    {
        return Verify("res://TestRefCounted.tscn");
    }

    [Fact]
    public Task RefCountedFromCSharp()
    {
        return Verify("res://TestRefCountedFromCSharp.tscn");
    }
}
