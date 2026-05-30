using Godot;

namespace NS;

public enum MyEnum { A, B, C }

[GodotClass]
public sealed partial class NodeWithSignalsSealed : Node
{
    public delegate void UnexposedDelegate();

    [Signal]
    public delegate void MySignalEventHandler();

    [Signal(Name = "my_named_signal")]
    public delegate void MyNamedSignalEventHandler();

    [Signal]
    public delegate void MySignalWithParametersEventHandler(int a, float b, string c, MyEnum d);

    [Signal]
    public delegate void MySignalWithNamedParametersEventHandler([BindProperty(Name = "my_number")] int myNumber, [BindProperty(Name = "my_string")] string myString);

    [Signal]
    public delegate void MySignalWithOptionalParametersEventHandler(int requiredParameter, int optionalParameter = 42);
}
