namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestGodotObjectToString : Control
{
    public override string ToString()
    {
        return $"[ GDExtension::Example <--> Instance ID:{GetInstanceId()} ]";
    }
}

[GodotClass]
public partial class TestGodotObjectNoToString : Control { }
