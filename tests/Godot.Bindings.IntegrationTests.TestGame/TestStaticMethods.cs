using Godot.Collections;

namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestStaticMethods : Control
{
    private static Variant _voidValue;

    [BindMethod(Name = "test_static")]
    public static int TestStatic(int a, int b)
    {
        return a + b;
    }

    [BindMethod(Name = "test_static_void")]
    public static void TestStaticVoid()
    {
        _voidValue = (GodotArray)(["static_func_void", 2]);
    }

    [BindMethod(Name = "get_void_value")]
    public static Variant GetVoidValue()
    {
        return _voidValue;
    }
}
