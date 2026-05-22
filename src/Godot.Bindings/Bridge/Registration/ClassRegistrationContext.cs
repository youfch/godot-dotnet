using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Godot.Bridge;

/// <summary>
/// Context for registering classes and their members within the Godot engine.
/// </summary>
public sealed partial class ClassRegistrationContext : IDisposable
{
    private bool _disposed;

    private GCHandle<ClassRegistrationContext> _gcHandle;

    internal GCHandle<ClassRegistrationContext> GCHandle => _gcHandle;

    internal StringName ClassName { get; }

    internal StringName NativeClassName { get; }

    private readonly ConcurrentQueue<Action> _registerBindingActions = [];

    internal ClassRegistrationContext(StringName className, StringName nativeClassName)
    {
        _gcHandle = new GCHandle<ClassRegistrationContext>(this);
        ClassName = className;
        NativeClassName = nativeClassName;
    }

    internal void RegisterBindings()
    {
        while (_registerBindingActions.TryDequeue(out var register))
        {
            register();
        }
    }

    /// <summary>
    /// Disposes of this <see cref="ClassRegistrationContext"/>.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        foreach (var handle in _registeredMethodHandles.Values)
        {
            handle.Dispose();
        }
        _registeredMethodHandles.Clear();

        _gcHandle.Dispose();
    }
}
