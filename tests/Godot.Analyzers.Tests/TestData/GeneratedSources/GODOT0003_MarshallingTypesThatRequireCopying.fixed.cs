using Godot;
using Godot.Collections;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NS;

[GodotClass]
public partial class MyNode : Node
{
    public List<int> MyUnboundField;

    [BindProperty]
    public PackedInt32Array MyField1;

    [BindProperty]
    public PackedInt32Array MyField2;

    [BindProperty]
    public PackedInt32Array MyField3;

    [BindProperty]
    public GodotArray<int> MyField4;

    public List<int> MyUnboundProperty { get; set; }

    [BindProperty]
    public PackedInt32Array MyProperty { get; set; }

    [BindProperty]
    public PackedInt32Array MyImmutableProperty { get; set; }

    [BindProperty]
    public PackedInt32Array MyReadOnlyListProperty { get; set; }

    public void MyUnboundMethod(List<int> parameter) { }

    [BindMethod]
    public void MyMethod(PackedInt32Array parameter) { }

    [BindMethod]
    public PackedInt32Array MyMethodWithReturn() => [];

    [BindMethod]
    public PackedInt32Array MyMethodWithImmutableReturn() => [];

    [BindMethod]
    public void MyMethodWithIListParameter(PackedInt32Array parameter) { }

    [BindMethod]
    public PackedInt32Array MyMethodWithIEnumerableReturn() => [];

    [BindProperty]
    public PackedInt32Array MyReadOnlyCollectionProperty { get; set; }

    [BindMethod]
    public void MyMethodWithICollectionParameter(PackedInt32Array parameter) { }

    [BindMethod]
    public PackedInt32Array MyMethodWithCollectionReturn() => [];

    [BindProperty]
    public GodotDictionary<int, int> MyDictionaryProperty { get; set; }

    [BindProperty]
    public GodotDictionary<int, int> MyReadOnlyDictionaryProperty { get; set; }

    [BindMethod]
    public void MyMethodWithIDictionaryParameter(GodotDictionary<int, int> parameter) { }

    [BindMethod]
    public GodotDictionary<int, int> MyMethodWithImmutableDictionaryReturn() => [];

    [Signal]
    public delegate void MySignalWithReadOnlyCollectionEventHandler(PackedInt32Array parameter);

    [Signal]
    public delegate void MySignalEventHandler(PackedInt32Array parameter);

    [BindMethod]
    public void MethodWithManyParameters(int a, string b, PackedInt32Array c, float d) { }

    [BindMethod]
    public int MethodWithManyParametersAndReturn1(int a, string b, PackedStringArray c, float d) => 0;

    [BindMethod]
    public PackedVector2Array MethodWithManyParametersAndReturn2(int a, string b, GodotArray<Node> c, float d) => [];
}
