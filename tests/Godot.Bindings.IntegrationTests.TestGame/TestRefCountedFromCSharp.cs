namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestRefCountedFromCSharp : TestBase
{
    protected override void _Ready()
    {
        // Test reference count for a new RefCounted starts at 1.
        using (var exampleRefCounted = new TestRefCountedFromCSharp_ExampleRef())
        {
            int refCount = exampleRefCounted.GetReferenceCount();
            AssertEqual(1, refCount);
        }

        // Test reference count for a new RefCounted starts at 1.
        using (var file = FileAccess.Open("res://empty.txt", FileAccess.ModeFlags.Read))
        {
            int refCount = file.GetReferenceCount();
            AssertEqual(1, refCount);
        }

        // Test reference count for a new RefCounted to the same resource is still 1.
        using (var file = FileAccess.Open("res://empty.txt", FileAccess.ModeFlags.Read))
        {
            int refCount = file.GetReferenceCount();
            AssertEqual(1, refCount);
        }

        // Test reference count for multiple RefCounted instances to the same resource is the same.
        using (var file1 = FileAccess.Open("res://empty.txt", FileAccess.ModeFlags.Read))
        using (var file2 = FileAccess.Open("res://empty.txt", FileAccess.ModeFlags.Read))
        {
            int refCount1 = file1.GetReferenceCount();
            int refCount2 = file2.GetReferenceCount();
            AssertEqual(1, refCount1);
            AssertEqual(1, refCount2);
        }

        // Test RefCounted wrapped in a Variant.
        using (var exampleRefCounted = new TestRefCountedFromCSharp_ExampleRef())
        {
            int refCount = exampleRefCounted.GetReferenceCount();
            AssertEqual(1, refCount);

            using (Variant variant = exampleRefCounted)
            {
                AssertEqual(2, exampleRefCounted.GetReferenceCount());

                // Test that the RefCounted can be extracted from the Variant and is the same instance.
                RefCounted? refCountedFromVariant = variant.AsGodotObject() as RefCounted;
                AssertNotEqual(null, refCountedFromVariant);
                AssertEqual(exampleRefCounted, refCountedFromVariant);
                AssertEqual(2, exampleRefCounted.GetReferenceCount());
            }
            AssertEqual(1, exampleRefCounted.GetReferenceCount());
        }

        ExitWithStatus();
    }
}
