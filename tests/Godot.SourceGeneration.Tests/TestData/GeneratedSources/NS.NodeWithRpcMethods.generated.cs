#nullable enable

namespace NS;

partial class @NodeWithRpcMethods
{
    public new partial class MethodName : global::Godot.Node.MethodName
    {
        public static global::Godot.StringName @MyRpcMethod { get; } = global::Godot.StringName.CreateStaticFromAscii("MyRpcMethod"u8);
        public static global::Godot.StringName @MyRpcMethodWithMode { get; } = global::Godot.StringName.CreateStaticFromAscii("MyRpcMethodWithMode"u8);
        public static global::Godot.StringName @MyFullyConfiguredRpcMethod { get; } = global::Godot.StringName.CreateStaticFromAscii("MyFullyConfiguredRpcMethod"u8);
    }
    public new partial class ConstantName : global::Godot.Node.ConstantName
    {
    }
    public new partial class PropertyName : global::Godot.Node.PropertyName
    {
    }
    public new partial class SignalName : global::Godot.Node.SignalName
    {
    }
#pragma warning disable CS0108 // Method might already be defined higher in the hierarchy, that's not an issue.
    internal static void BindMembers(global::Godot.Bridge.ClassRegistrationContext context)
#pragma warning restore CS0108 // Method might already be defined higher in the hierarchy, that's not an issue.
    {
        context.BindConstructor(() => new global::NS.NodeWithRpcMethods());
        context.BindMethod(MethodName.@MyRpcMethod,
            static (NodeWithRpcMethods __instance) =>
            {
                __instance.@MyRpcMethod();
            });
        context.BindMethod(MethodName.@MyRpcMethodWithMode,
            static (NodeWithRpcMethods __instance) =>
            {
                __instance.@MyRpcMethodWithMode();
            });
        context.BindMethod(MethodName.@MyFullyConfiguredRpcMethod,
            static (NodeWithRpcMethods __instance) =>
            {
                __instance.@MyFullyConfiguredRpcMethod();
            });
        context.SetRpcConfig(MethodName.@MyRpcMethod, new global::Godot.NativeInterop.RpcConfig()
        {
            Mode = global::Godot.MultiplayerApi.RpcMode.Authority,
            CallLocal = false,
            TransferMode = global::Godot.MultiplayerPeer.TransferModeEnum.Reliable,
            TransferChannel = 0,
        });
        context.SetRpcConfig(MethodName.@MyRpcMethodWithMode, new global::Godot.NativeInterop.RpcConfig()
        {
            Mode = global::Godot.MultiplayerApi.RpcMode.AnyPeer,
            CallLocal = false,
            TransferMode = global::Godot.MultiplayerPeer.TransferModeEnum.Reliable,
            TransferChannel = 0,
        });
        context.SetRpcConfig(MethodName.@MyFullyConfiguredRpcMethod, new global::Godot.NativeInterop.RpcConfig()
        {
            Mode = global::Godot.MultiplayerApi.RpcMode.Authority,
            CallLocal = true,
            TransferMode = global::Godot.MultiplayerPeer.TransferModeEnum.Unreliable,
            TransferChannel = 1,
        });
    }
}
