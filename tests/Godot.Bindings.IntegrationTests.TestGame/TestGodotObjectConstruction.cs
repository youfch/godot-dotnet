using Godot.Bridge;

namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestGodotObjectConstruction : Control
{
    private readonly bool _objectInstanceBindingsSetByParentConstructor;

    private unsafe bool HasObjectInstanceBindings()
    {
        return GodotBridge.GDExtensionInterface.object_get_instance_binding((void*)NativePtr, GodotBridge.LibraryPtr, null) is not null;
    }

    public TestGodotObjectConstruction()
    {
        _objectInstanceBindingsSetByParentConstructor = HasObjectInstanceBindings();
    }

    [BindMethod(Name = "is_object_bindings_set_by_parent_constructor")]
    public bool IsObjectBindingsSetByParentConstructor()
    {
        return _objectInstanceBindingsSetByParentConstructor;
    }

    [BindMethod(Name = "test_post_initialize")]
    public static bool TestPostInitialize()
    {
        var exampleRef = new TestGodotObjectConstruction_ExampleRef();
        return exampleRef.WasPostInitialized();
    }
}
