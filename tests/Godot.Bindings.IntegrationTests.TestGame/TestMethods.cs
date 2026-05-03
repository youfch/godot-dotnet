namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestMethods : Control
{
    public partial class MethodName
    {
        public static StringName Test { get; } = "test";
    }

    [BindMethod(Name = "custom_ref_func")]
    public int CustomRefFunc(TestMethods_ExampleRef? @ref)
    {
        return @ref is not null ? @ref.Id : -1;
    }

    [BindMethod(Name = "image_ref_func")]
    public string ImageRefFunc(Image? image)
    {
        return image is not null ? "valid" : "invalid";
    }

    [BindMethod(Name = "get_v4")]
    public Vector4 GetVector4()
    {
        return new Vector4(1.2f, 3.4f, 5.6f, 7.8f);
    }

    [BindMethod(Name = "test_node_argument")]
    public Node TestNodeArgument(Node node)
    {
        return node;
    }

    [BindMethod(Name = "return_something")]
    public string ReturnSomething(string @base)
    {
        return $"{@base}42";
    }

    [BindMethod(Name = "return_something_const")]
    public Viewport? ReturnSomethingConst()
    {
        if (IsInsideTree())
        {
            Viewport result = GetViewport();
            return result;
        }

        return null;
    }

    [BindMethod(Name = "return_empty_ref")]
    public TestMethods_ExampleRef? ReturnEmptyRef()
    {
        return null;
    }

    [BindMethod(Name = "return_extended_ref")]
    public TestMethods_ExampleRef ReturnExtendedRef()
    {
        return new TestMethods_ExampleRef();
    }

    [BindMethod(Name = "extended_ref_checks")]
    public TestMethods_ExampleRef ExtendedRefChecks(TestMethods_ExampleRef @ref)
    {
        return new TestMethods_ExampleRef();
    }

    [BindMethod(Name = "test_add_child")]
    public void TestAddChild(Node child)
    {
        AddChild(child);
    }

    [BindMethod(Name = "test_set_tileset")]
    public void TestSetTileset(TileMap tilemap, TileSet tileset)
    {
        tilemap.SetTileset(tileset);
    }

    [BindMethod(Name = "test_object_call")]
    public Variant TestObjectCall(GodotObject input)
    {
        return input.Call(MethodName.Test, "hello");
    }
}
