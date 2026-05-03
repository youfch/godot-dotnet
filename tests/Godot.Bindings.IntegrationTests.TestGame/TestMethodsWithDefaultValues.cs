namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestMethodsWithDefaultValues : Control
{
    [BindMethod(Name = "def_args")]
    public int DefArgs(int a = 100, int b = 200)
    {
        return a + b;
    }
}
