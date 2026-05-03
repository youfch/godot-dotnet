namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestGodotObjectInstanceIsValid : Node
{
    [BindMethod(Name = "test_object_is_valid")]
    public bool TestObjectIsValid(GodotObject? obj)
    {
        return IsInstanceValid(obj);
    }
}
