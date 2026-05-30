namespace Godot.Bindings.IntegrationTests.TestGame;

public enum EnumTest { A, B, C }

[GodotClass]
public partial class TestSignalConnection : TestBase
{
    [Signal]
    public delegate void SignalWithoutParametersEventHandler();

    [Signal]
    public delegate void SignalWithParametersEventHandler(int a, float b, string c, EnumTest d);

    protected override void _Ready()
    {
        // Connect signals using Connect/Disconnect.
        {
            Callable callable = Callable.From(OnSignalWithoutParameters);
            AssertFalse(IsConnected(SignalName.SignalWithoutParameters, callable));
            Connect(SignalName.SignalWithoutParameters, callable);
            AssertTrue(IsConnected(SignalName.SignalWithoutParameters, callable));
            Disconnect(SignalName.SignalWithoutParameters, callable);
            AssertFalse(IsConnected(SignalName.SignalWithoutParameters, callable));
        }
        {
            Callable callable = Callable.From<int, float, string, EnumTest>(OnSignalWithParameters);
            AssertFalse(IsConnected(SignalName.SignalWithParameters, callable));
            Connect(SignalName.SignalWithParameters, callable);
            AssertTrue(IsConnected(SignalName.SignalWithParameters, callable));
            Disconnect(SignalName.SignalWithParameters, callable);
            AssertFalse(IsConnected(SignalName.SignalWithParameters, callable));
        }

        // Connect signals using C# events.
        {
            SignalWithoutParametersEventHandler handler = OnSignalWithoutParameters;
            Callable callable = Callable.From(handler.Invoke);
            AssertFalse(IsConnected(SignalName.SignalWithoutParameters, callable));
            SignalWithoutParameters += handler;
            AssertTrue(IsConnected(SignalName.SignalWithoutParameters, callable));
            SignalWithoutParameters -= handler;
            AssertFalse(IsConnected(SignalName.SignalWithoutParameters, callable));
        }
        {
            SignalWithParametersEventHandler handler = OnSignalWithParameters;
            Callable callable = Callable.From<int, float, string, EnumTest>(handler.Invoke);
            AssertFalse(IsConnected(SignalName.SignalWithParameters, callable));
            SignalWithParameters += handler;
            AssertTrue(IsConnected(SignalName.SignalWithParameters, callable));
            SignalWithParameters -= handler;
            AssertFalse(IsConnected(SignalName.SignalWithParameters, callable));
        }

        ExitWithStatus();
    }

    private void OnSignalWithoutParameters() { }
    private void OnSignalWithParameters(int a, float b, string c, EnumTest d) { }
}
