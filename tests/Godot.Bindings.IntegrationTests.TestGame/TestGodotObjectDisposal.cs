using System;

namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestGodotObjectDisposal : TestBase
{
    public static class NodePaths
    {
        public static readonly NodePath Child = nameof(Child);
    }

    protected override void _Ready()
    {
        var child1 = GetNode(NodePaths.Child);
        var child2 = GetNode(NodePaths.Child);
        AssertEqual(child1, child2);

        child1.Dispose();
        AssertThrow<ObjectDisposedException>(() => child2.GetName());

        var child3 = GetNode(NodePaths.Child);
        AssertNotEqual(child1, child3);
        AssertNotThrow(() => child3.GetName());

        ExitWithStatus();
    }
}
