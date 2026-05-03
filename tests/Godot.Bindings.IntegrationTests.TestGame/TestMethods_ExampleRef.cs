namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestMethods_ExampleRef : RefCounted
{
    [BindProperty(Name = "id")]
    public int Id { get; set; }
}
