using Godot.Collections;

namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestVirtualMethods : Control
{
    private Variant _virtualValue;

    protected override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent)
        {
            _virtualValue = (GodotArray)([$"_input: {keyEvent.GetKeyLabel()}", keyEvent.Unicode.Value]);
        }
    }

    [BindMethod(Name = "get_virtual_value")]
    public Variant GetVirtualValue()
    {
        return _virtualValue;
    }

    [BindMethod(Name = "test_virtual_implemented_in_script")]
    public string TestVirtualImplementedInScript(string name, int value)
    {
        return TestVirtualImplementedInScriptCore(name, value);
    }

    [BindMethod(Name = "_do_something_virtual", Virtual = true)]
    private string TestVirtualImplementedInScriptCore(string name, int value)
    {
        if (TryCallVirtualMethod<string, int, string>(MethodName.TestVirtualImplementedInScriptCore, name, value, out string? result))
        {
            return result;
        }

        return "Unimplemented";
    }
}
