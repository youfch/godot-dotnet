namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestRpc : Control
{
    private int _lastRpcValue;

    [BindMethod(Name = "return_last_rpc_arg")]
    public int ReturnLastRpcValue()
    {
        return _lastRpcValue;
    }

    [Rpc(MultiplayerApi.RpcMode.Authority,
        TransferMode = MultiplayerPeer.TransferModeEnum.Reliable,
        CallLocal = true,
        TransferChannel = 0)]
    [BindMethod(Name = "test_rpc")]
    public void TestRpcLocal(int value)
    {
        _lastRpcValue = value;
    }

    [BindMethod(Name = "test_send_rpc")]
    public void TestRpcSend(int value)
    {
        Rpc(MethodName.TestRpcLocal, value);
    }

    // TODO: There's a bug where if the _Ready method is not overridden, the RPC methods are not registered. This is because we intentionally avoid calling virtual methods that aren't overridden for performance, but it's the only place where we could register the RPC methods.
    protected override void _Ready() { }
}
