namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestGodotObjectNotifications_Base : Node
{
    protected int Value1 { get; set; }
    protected int Value2 { get; set; }

    [BindMethod(Name = "get_value1")]
    private int GetValue1() => Value1;

    [BindMethod(Name = "get_value2")]
    private int GetValue2() => Value2;

    protected internal override void _Notification(int what)
    {
        if (what == NotificationEnterTree)
        {
            Value1 = 11;
            Value2 = 22;
        }
    }
}

[GodotClass]
public partial class TestGodotObjectNotifications_Derived : TestGodotObjectNotifications_Base
{
    protected internal override void _Notification(int what)
    {
        if (what == NotificationEnterTree)
        {
            Value2 = 33;
        }
    }
}
