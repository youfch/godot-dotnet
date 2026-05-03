namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestStrings : RefCounted
{
    [BindMethod(Name = "test_string_unicode_in")]
    public static bool TestStringUnicodeIn(string input)
    {
        return input == "ABCĎE";
    }

    [BindMethod(Name = "test_string_unicode_out")]
    public static string TestStringUnicodeOut()
    {
        return "ABCĎE";
    }
}
