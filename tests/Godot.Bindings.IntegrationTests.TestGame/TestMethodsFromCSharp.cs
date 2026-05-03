namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestMethodsFromCSharp : TestBase
{
    protected override void _Ready()
    {
        Call(MethodName.Method);
        PropagateCall(MethodName.PropagatedMethodEmpty, []);
        PropagateCall(MethodName.PropagatedMethodNull, null);

        ExitWithStatus();
    }

    [BindMethod]
    public void Method()
    {
        GD.Print("Method called");
    }

    [BindMethod]
    public void PropagatedMethodEmpty()
    {
        GD.Print("PropagatedMethodEmpty called");
    }

    [BindMethod]
    public void PropagatedMethodNull()
    {
        GD.Print("PropagatedMethodNull called");
    }
}
