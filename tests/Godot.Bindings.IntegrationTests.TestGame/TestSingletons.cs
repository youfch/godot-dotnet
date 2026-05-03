namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestSingletons : RefCounted
{
    [BindMethod(Name = "test_use_engine_singleton")]
    public static string TestUseEngineSingleton()
    {
        return OS.Singleton.GetName();
    }
}
