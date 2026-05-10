using Godot.Collections;

namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestRefCounted : RefCounted
{
    [BindMethod(Name = "create_from_csharp")]
    public static TestRefCounted Create() => new TestRefCounted();

    [BindMethod(Name = "test_get_reference_count")]
    public static int TestGetReferenceCount(RefCounted refCounted)
    {
        return refCounted.GetReferenceCount();
    }

    [BindMethod(Name = "test_get_reference_count_for_object")]
    public static int TestGetReferenceCountForObject(GodotObject obj)
    {
        if (obj is RefCounted refCounted)
        {
            return refCounted.GetReferenceCount();
        }

        // Fail the assert.
        return -1;
    }

    [BindMethod(Name = "test_get_reference_count_for_variant")]
    public static int TestGetReferenceCountForVariant(Variant variant)
    {
        if (variant.AsGodotObject() is RefCounted refCounted)
        {
            return refCounted.GetReferenceCount();
        }

        // Fail the assert.
        return -1;
    }

    [BindMethod(Name = "test_create_and_get_refcount")]
    public static int TestCreateAndGetReferenceCount()
    {
        var curve = new Curve2D();
        return curve.GetReferenceCount();
    }

    [BindMethod(Name = "test_multiple_refcounted_same_reference")]
    public static GodotArray TestMultipleRefCountedSameReference()
    {
        using var file1 = FileAccess.Open("res://empty.txt", FileAccess.ModeFlags.Read);
        using var file2 = FileAccess.Open("res://empty.txt", FileAccess.ModeFlags.Read);
        return [file1.GetReferenceCount(), file2.GetReferenceCount()];
    }

    [BindMethod(Name = "test_dispose_refcounted")]
    public static void TestDisposeRefCounted(RefCounted refCounted)
    {
        refCounted.Dispose();
    }

    [BindMethod(Name = "test_dispose_object")]
    public static void TestDisposeObject(GodotObject obj)
    {
        obj.Dispose();
    }

    [BindMethod(Name = "test_get_type_name")]
    public static string TestGetTypeName(GodotObject obj)
    {
        return obj.GetType().Name;
    }
}
