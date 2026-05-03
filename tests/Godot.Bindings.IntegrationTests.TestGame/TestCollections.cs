using Godot.Collections;

namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestCollections : GodotObject
{
    [BindMethod(Name = "test_array")]
    public static GodotArray TestArray()
    {
        return [1, 2];
    }

    [BindMethod(Name = "test_tarray")]
    public static GodotArray<Vector2> TestTypedArray()
    {
        return
        [
            new Vector2(1, 2),
            new Vector2(2, 3),
        ];
    }

    [BindMethod(Name = "test_tarray_arg")]
    public static int TestTypedArrayArg(GodotArray<long> array)
    {
        int sum = 0;
        foreach (long value in array)
        {
            sum += (int)value;
        }
        return sum;
    }

    [BindMethod(Name = "test_dictionary")]
    public static GodotDictionary TestDictionary()
    {
        return new GodotDictionary()
        {
            ["hello"] = "world",
            ["foo"] = "bar"
        };
    }

    [BindMethod(Name = "test_tdictionary")]
    public static GodotDictionary<Vector2, Vector2I> TestTypedDictionary()
    {
        return new GodotDictionary<Vector2, Vector2I>()
        {
            [new Vector2(1, 2)] = new Vector2I(2, 3),
        };
    }

    [BindMethod(Name = "test_tdictionary_arg")]
    public static int TestTypedDictionaryArg(GodotDictionary<string, long> dictionary)
    {
        int sum = 0;
        foreach (var (_, value) in dictionary)
        {
            sum += (int)value;
        }
        return sum;
    }

    [BindMethod(Name = "test_vector_ops")]
    public static int TestPackedInt32Array()
    {
        PackedInt32Array array = [10, 20, 30, 45];
        int ret = 0;
        foreach (int value in array)
        {
            ret += value;
        }
        return ret;
    }
}
