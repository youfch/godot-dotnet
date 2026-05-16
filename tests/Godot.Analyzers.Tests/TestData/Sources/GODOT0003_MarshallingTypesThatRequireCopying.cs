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
    public {|GODOT0003:List<int>|} MyField1;

    [BindProperty]
    public {|GODOT0003:int[]|} MyField2;

    [BindProperty]
    public PackedInt32Array MyField3;

    [BindProperty]
    public GodotArray<int> MyField4;

    public List<int> MyUnboundProperty { get; set; }

    [BindProperty]
    public {|GODOT0003:List<int>|} MyProperty { get; set; }

    [BindProperty]
    public {|GODOT0003:ImmutableArray<int>|} MyImmutableProperty { get; set; }

    [BindProperty]
    public {|GODOT0003:IReadOnlyList<int>|} MyReadOnlyListProperty { get; set; }

    public void MyUnboundMethod(List<int> parameter) { }

    [BindMethod]
    public void MyMethod({|GODOT0003:List<int>|} parameter) { }

    [BindMethod]
    public {|GODOT0003:List<int>|} MyMethodWithReturn() => [];

    [BindMethod]
    public {|GODOT0003:ImmutableArray<int>|} MyMethodWithImmutableReturn() => [];

    [BindMethod]
    public void MyMethodWithIListParameter({|GODOT0003:IList<int>|} parameter) { }

    [BindMethod]
    public {|GODOT0003:IEnumerable<int>|} MyMethodWithIEnumerableReturn() => [];

    [BindProperty]
    public {|GODOT0003:IReadOnlyCollection<int>|} MyReadOnlyCollectionProperty { get; set; }

    [BindMethod]
    public void MyMethodWithICollectionParameter({|GODOT0003:ICollection<int>|} parameter) { }

    [BindMethod]
    public {|GODOT0003:Collection<int>|} MyMethodWithCollectionReturn() => [];

    [BindProperty]
    public {|GODOT0003:Dictionary<int, int>|} MyDictionaryProperty { get; set; }

    [BindProperty]
    public {|GODOT0003:IReadOnlyDictionary<int, int>|} MyReadOnlyDictionaryProperty { get; set; }

    [BindMethod]
    public void MyMethodWithIDictionaryParameter({|GODOT0003:IDictionary<int, int>|} parameter) { }

    [BindMethod]
    public {|GODOT0003:ImmutableDictionary<int, int>|} MyMethodWithImmutableDictionaryReturn() => [];

    [Signal]
    public delegate void MySignalWithReadOnlyCollectionEventHandler({|GODOT0003:ReadOnlyCollection<int>|} parameter);

    [Signal]
    public delegate void MySignalEventHandler({|GODOT0003:List<int>|} parameter);

    [BindMethod]
    public void MethodWithManyParameters(int a, string b, {|GODOT0003:int[]|} c, float d) { }

    [BindMethod]
    public int MethodWithManyParametersAndReturn1(int a, string b, {|GODOT0003:string[]|} c, float d) => 0;

    [BindMethod]
    public {|GODOT0003:Vector2[]|} MethodWithManyParametersAndReturn2(int a, string b, {|GODOT0003:Node[]|} c, float d) => [];
}
