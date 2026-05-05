using System.Collections.Generic;
using System.Threading;
using Godot.NativeInterop;

namespace Godot.Bridge;

partial class ClassRegistrationContext
{
    private readonly Dictionary<StringName, RpcConfig> _rpcConfigs = new(StringNameEqualityComparer.Default);

    private bool _rpcConfigSet;

    /// <summary>
    /// Set the RPC configuration for a method in the class.
    /// If a configuration has already been set for <paramref name="methodName"/>,
    /// it will be silently replaced by the new configuration.
    /// </summary>
    /// <param name="methodName">Name of the method to configure.</param>
    /// <param name="rpcConfig">The RPC configuration to set for the method.</param>
    public void SetRpcConfig(StringName methodName, RpcConfig rpcConfig)
    {
        _rpcConfigs[methodName] = rpcConfig;
    }

    internal void RegisterRpcMethods(Node instance)
    {
        // We only need to set the RPC configuration once per class,
        // so we check if it's already been set before iterating through the methods.
        if (Interlocked.Exchange(ref _rpcConfigSet, true))
        {
            return;
        }

        foreach (var (methodName, rpcConfig) in _rpcConfigs)
        {
            instance.RpcConfig(methodName, rpcConfig.GetConfigDictionary());
        }
    }
}
