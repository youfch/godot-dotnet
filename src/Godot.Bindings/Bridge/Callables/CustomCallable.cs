using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot.NativeInterop;

namespace Godot.Bridge;

/// <summary>
/// Implements the custom behavior for an user-implemented <see cref="Callable"/>.
/// Most users should use <see cref="Callable"/> directly, this is a low-level
/// API to allow implementing custom behavior for special cases and it's used
/// internally to implement delegate-based Callables.
/// </summary>
public abstract class CustomCallable : IDisposable
{
    private bool _disposed;

    private GCHandle<CustomCallable> _gcHandle;

    /// <summary>
    /// Constructs a <see cref="CustomCallable"/>.
    /// </summary>
    public CustomCallable()
    {
        _gcHandle = new GCHandle<CustomCallable>(this);

        DisposablesTracker.RegisterDisposable(this);
    }

    /// <summary>
    /// Convert a <see cref="CustomCallable"/> into a <see cref="Callable"/>.
    /// </summary>
    public static implicit operator Callable(CustomCallable customCallable)
    {
        return Callable.CreateTakingOwnership(customCallable);
    }

    internal unsafe NativeGodotCallable ConstructCallable()
    {
        var info = new GDExtensionCallableCustomInfo2()
        {
            callable_userdata = (void*)GCHandle<CustomCallable>.ToIntPtr(_gcHandle),
            token = GodotBridge.LibraryPtr,
            object_id = GetObjectId(),
            call_func = &Call_Native,
            is_valid_func = &IsValid_Native,
            free_func = &Free_Native,
            hash_func = &Hash_Native,
            equal_func = &Equals_Native,
            less_than_func = &LessThan_Native,
            to_string_func = &ToString_Native,
            get_argument_count_func = &GetArgumentCount_Native,
        };

        NativeGodotCallable callable = default;
        GodotBridge.GDExtensionInterface.callable_custom_create2(&callable, &info);
        return callable;
    }

    /// <summary>
    /// The instance ID of the object that is the owner of this callable.
    /// </summary>
    /// <returns>Instance ID of the callable's owner.</returns>
    protected abstract ulong GetObjectId();

    /// <summary>
    /// Determines whether this callable is still valid.
    /// </summary>
    /// <returns><see langword="true"/> if the callable is valid.</returns>
    protected virtual bool IsValid() => true;

    /// <summary>
    /// Try to retrieve the argument count required by this callable.
    /// </summary>
    /// <param name="argCount">The number of parameters of the function.</param>
    /// <returns><see langword="true"/> if the argument count was retrieved successfully.</returns>
    protected virtual bool TryGetArgumentCount(out long argCount)
    {
        argCount = 0;
        return false;
    }

#pragma warning disable CA1707 // Identifiers should not contain underscores.
#pragma warning disable IDE1006 // Naming Styles.
    /// <summary>
    /// Implements the callback that will be invoked when this callable is called.
    /// </summary>
    /// <param name="args">Arguments that this callable is invoked with.</param>
    /// <param name="result">The value returned by the callable's invocation.</param>
    /// <returns>
    /// A status that indicates whether the call was successful, or the error that occurred otherwise.
    /// </returns>
    protected abstract CallError _Call(ReadOnlySpan<Variant> args, out Variant result);
#pragma warning restore IDE1006 // Naming Styles.
#pragma warning restore CA1707 // Identifiers should not contain underscores.

    internal virtual unsafe void Call(NativeGodotVariantPtrSpan args, NativeGodotVariant* outRet, GDExtensionCallError* outError)
    {
        Variant[] variantArgs = new Variant[args.Length];
        for (int i = 0; i < args.Length; i++)
        {
            variantArgs[i] = Variant.CreateCopying(args[i]);
        }

        CallError status = _Call(variantArgs, out Variant result);
        if (status != CallError.Ok)
        {
            outError->error = (GDExtensionCallErrorType)status;
            return;
        }

        *outRet = NativeGodotVariant.Create(result.NativeValue.DangerousSelfRef);
        outError->error = GDExtensionCallErrorType.GDEXTENSION_CALL_OK;
    }

    /// <summary>
    /// Releases the unmanaged <see cref="CustomCallable"/> instance.
    /// </summary>
    ~CustomCallable()
    {
        Dispose(false);
    }

    /// <summary>
    /// Disposes of this <see cref="CustomCallable"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes implementation of this <see cref="CustomCallable"/>.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        _gcHandle.Dispose();
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void Call_Native(void* userData, NativeGodotVariant** args, long argsCount, NativeGodotVariant* outRet, GDExtensionCallError* outError)
    {
        try
        {
            var gcHandle = GCHandle<CustomCallable>.FromIntPtr((nint)userData);
            var callable = gcHandle.Target;

            if (callable is null)
            {
                outError->error = GDExtensionCallErrorType.GDEXTENSION_CALL_ERROR_INSTANCE_IS_NULL;
                return;
            }

            callable.Call(new NativeGodotVariantPtrSpan(args, (int)argsCount), outRet, outError);
        }
        catch (Exception exception) when (ExceptionHandling.IsHandled(exception))
        {
            outError->error = GDExtensionCallErrorType.GDEXTENSION_CALL_ERROR_INVALID_METHOD;
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe bool IsValid_Native(void* userData)
    {
        try
        {
            var gcHandle = GCHandle<CustomCallable>.FromIntPtr((nint)userData);
            var callable = gcHandle.Target;

            return callable.IsValid();
        }
        catch (Exception exception) when (ExceptionHandling.IsHandled(exception))
        {
            return false;
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void Free_Native(void* userData)
    {
        try
        {
            var gcHandle = GCHandle<CustomCallable>.FromIntPtr((nint)userData);
            if (gcHandle.Target is IDisposable disposable)
            {
                disposable.Dispose();
            }
            gcHandle.Dispose();
        }
        catch (Exception exception) when (ExceptionHandling.IsHandled(exception)) { }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe uint Hash_Native(void* userData)
    {
        try
        {
            var gcHandle = GCHandle<CustomCallable>.FromIntPtr((nint)userData);
            var callable = gcHandle.Target;

            return (uint)callable.GetHashCode();
        }
        catch (Exception exception) when (ExceptionHandling.IsHandled(exception))
        {
            return 0;
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe bool Equals_Native(void* userDataLeft, void* userDataRight)
    {
        try
        {
            var gcHandleLeft = GCHandle<CustomCallable>.FromIntPtr((nint)userDataLeft);
            var callableLeft = gcHandleLeft.Target;

            var gcHandleRight = GCHandle<CustomCallable>.FromIntPtr((nint)userDataRight);
            var callableRight = gcHandleRight.Target;

            return EqualityComparer<CustomCallable>.Default.Equals(callableLeft, callableRight);
        }
        catch (Exception exception) when (ExceptionHandling.IsHandled(exception))
        {
            return false;
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe bool LessThan_Native(void* userDataLeft, void* userDataRight)
    {
        try
        {
            var gcHandleLeft = GCHandle<CustomCallable>.FromIntPtr((nint)userDataLeft);
            var callableLeft = gcHandleLeft.Target;

            var gcHandleRight = GCHandle<CustomCallable>.FromIntPtr((nint)userDataRight);
            var callableRight = gcHandleRight.Target;

            return Comparer<CustomCallable>.Default.Compare(callableLeft, callableRight) switch
            {
                // callableLeft is less than callableRight.
                < 0 => true,
                // callableLeft equals callableRight.
                0 => false,
                // callableLeft is greater than callableRight.
                > 0 => false,
            };
        }
        catch (Exception exception) when (ExceptionHandling.IsHandled(exception))
        {
            return false;
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void ToString_Native(void* userData, bool* outIsValid, NativeGodotString* outStr)
    {
        try
        {
            var gcHandle = GCHandle<CustomCallable>.FromIntPtr((nint)userData);
            var callable = gcHandle.Target;

            if (callable is null)
            {
                *outIsValid = false;
                *outStr = default;
            }
            else
            {
                *outIsValid = true;
                *outStr = NativeGodotString.Create(callable.ToString());
            }
        }
        catch (Exception exception) when (ExceptionHandling.IsHandled(exception))
        {
            *outIsValid = false;
            *outStr = default;
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe long GetArgumentCount_Native(void* userData, bool* outIsValid)
    {
        try
        {
            var gcHandle = GCHandle<CustomCallable>.FromIntPtr((nint)userData);
            var callable = gcHandle.Target;

            *outIsValid = callable.TryGetArgumentCount(out long argCount);
            return argCount;
        }
        catch (Exception exception) when (ExceptionHandling.IsHandled(exception))
        {
            *outIsValid = false;
            return 0;
        }
    }
}
