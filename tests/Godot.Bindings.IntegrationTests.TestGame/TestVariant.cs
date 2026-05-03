using System;
using Godot.Collections;

namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestVariant : GodotObject
{
    [BindMethod(Name = "test_variant_vector2i_conversion")]
    public static Vector2I TestVariantToVector2IConversion(Variant variant)
    {
        return variant.AsVector2I();
    }

    [BindMethod(Name = "test_variant_int_conversion")]
    public static int TestVariantToInt32Conversion(Variant variant)
    {
        return variant.AsInt32();
    }

    [BindMethod(Name = "test_variant_float_conversion")]
    public static float TestVariantToSingleConversion(Variant variant)
    {
        return variant.AsSingle();
    }

    [BindMethod(Name = "test_variant_iterator")]
    public static Variant TestVariantEnumerate(Variant variant)
    {
        try
        {
            GodotArray<int> result = [];

            foreach (Variant item in variant.Enumerate())
            {
                result.Add(item.AsInt32() + 5);
            }

            return result;
        }
        catch (InvalidOperationException)
        {
            return "iter_init: not valid";
        }
    }
}
