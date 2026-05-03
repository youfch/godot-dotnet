namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestGodotObjectUnicodeNamePrzykład : Control
{
    [BindMethod(Name = "get_the_word")]
    public string GetTheWord()
    {
        return "słowo to przykład";
    }
}
