using System;
using Godot.Bridge;
using Godot.Collections;

namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestCallables : Control
{
    private Variant _voidValue;

    [BindMethod(Name = "get_void_value")]
    public Variant GetVoidValue()
    {
        return _voidValue;
    }

    [BindMethod(Name = "test_callable_mp")]
    public Callable TestCallable()
    {
        return Callable.From<GodotObject, string, int>(UnboundMethod1);
    }

    [BindMethod(Name = "test_callable_mp_ret")]
    public Callable TestCallableWithReturn()
    {
        return Callable.From<GodotObject, string, int, string>(UnboundMethod2);
    }

    [BindMethod(Name = "test_callable_mp_static")]
    public Callable TestCallableStatic()
    {
        return Callable.From<TestCallables, string, int>(UnboundStaticMethod1);
    }

    [BindMethod(Name = "test_callable_mp_static_ret")]
    public Callable TestCallableStaticWithReturn()
    {
        return Callable.From<GodotObject, string, int, string>(UnboundStaticMethod2);
    }

    [BindMethod(Name = "test_custom_callable")]
    public Callable TestCustomCallable()
    {
        return new MyCustomCallable();
    }

    public void UnboundMethod1(GodotObject obj, string str, int i)
    {
        string test = $"unbound_method1: {obj.GetClass()} - {str}";
        _voidValue = (GodotArray)([test, i]);
    }

    public string UnboundMethod2(GodotObject obj, string str, int i)
    {
        string test = $"unbound_method2: {obj.GetClass()} - {str} - {i}";
        return test;
    }

    public static void UnboundStaticMethod1(TestCallables obj, string str, int i)
    {
        string test = $"unbound_static_method1: {obj.GetClass()} - {str}";
        obj._voidValue = (GodotArray)([test, i]);
    }

    public static string UnboundStaticMethod2(GodotObject obj, string str, int i)
    {
        string test = $"unbound_static_method2: {obj.GetClass()} - {str} - {i}";
        return test;
    }
}

internal sealed class MyCustomCallable : CustomCallable
{
    protected override bool IsValid() => true;

    protected override ulong GetObjectId() => 0;

    protected override bool TryGetArgumentCount(out long argCount)
    {
        argCount = 2;
        return true;
    }

    protected override CallError _Call(ReadOnlySpan<Variant> args, out Variant result)
    {
        result = "Hi";
        return CallError.Ok;
    }

    public override int GetHashCode()
    {
        return 27;
    }

    public override string ToString()
    {
        return "<MyCallableCustom>";
    }
}
