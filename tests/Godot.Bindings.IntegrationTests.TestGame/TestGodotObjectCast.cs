namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestGodotObjectCast : Control
{
    [BindMethod(Name = "test_object_is_node")]
    public static bool TestObjectIsNode(GodotObject obj)
    {
        return obj is Node;
    }

    [BindMethod(Name = "test_object_is_control")]
    public static bool TestObjectIsControl(GodotObject obj)
    {
        return obj is Control;
    }

    [BindMethod(Name = "test_object_is_self")]
    public static bool TestObjectIsSelf(GodotObject obj)
    {
        return obj is TestGodotObjectCast;
    }
}
