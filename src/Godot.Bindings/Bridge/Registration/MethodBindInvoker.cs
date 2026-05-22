using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot.NativeInterop;

namespace Godot.Bridge;

/// <summary>
/// Helper to invoke methods registered for a class with <see cref="ClassRegistrationContext"/>.
/// </summary>
public readonly partial struct MethodBindInvoker
{
    private readonly Delegate _delegate;

    private readonly unsafe delegate* managed<GodotObject, Delegate, void**, void*, void> _trampolineWithPtrArgs;
    private readonly unsafe delegate* managed<MethodDefinition, GodotObject, Delegate, NativeGodotVariantPtrSpan, out NativeGodotVariant, void> _trampolineWithVariantArgs;

    private unsafe MethodBindInvoker(Delegate @delegate,
        delegate* managed<GodotObject, Delegate, void**, void*, void> trampolineWithPtrArgs,
        delegate* managed<MethodDefinition, GodotObject, Delegate, NativeGodotVariantPtrSpan, out NativeGodotVariant, void> trampolineWithVariantArgs)
    {
        _delegate = @delegate;
        _trampolineWithPtrArgs = trampolineWithPtrArgs;
        _trampolineWithVariantArgs = trampolineWithVariantArgs;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe MethodBindInvoker CreateWithUnsafeTrampoline(Delegate @delegate,
        delegate* managed<GodotObject, Delegate, void**, void*, void> trampolineWithPtrArgs,
        delegate* managed<MethodDefinition, GodotObject, Delegate, NativeGodotVariantPtrSpan, out NativeGodotVariant, void> trampolineWithVariantArgs)
    {
        return new MethodBindInvoker(@delegate, trampolineWithPtrArgs, trampolineWithVariantArgs);
    }

    internal unsafe void CallWithPtrArgs(MethodDefinition methodDefinition, void* instance, void** args, void* outRet)
    {
        GodotObject instanceObj = GetInstanceObject(methodDefinition, instance);

        _trampolineWithPtrArgs(instanceObj, _delegate, args, outRet);
    }

    internal unsafe void CallWithVariantArgs(MethodDefinition methodDefinition, void* instance, NativeGodotVariantPtrSpan args, NativeGodotVariant* outRet, GDExtensionCallError* outError)
    {
        GodotObject instanceObj = GetInstanceObject(methodDefinition, instance);

        _trampolineWithVariantArgs(methodDefinition, instanceObj, _delegate, args, out NativeGodotVariant ret);
        *outRet = ret;

        outError->error = GDExtensionCallErrorType.GDEXTENSION_CALL_OK;
    }

    internal unsafe void CallVirtualWithPtrArgs(void* instance, void** args, void* outRet)
    {
        GodotObject instanceObj = GetInstanceObject(instance);

        _trampolineWithPtrArgs(instanceObj, _delegate, args, outRet);
    }

    private static unsafe GodotObject GetInstanceObject(MethodDefinition methodDefinition, void* instance)
    {
        if (methodDefinition.IsStatic)
        {
            // Static methods don't have an instance.
            Debug.Assert(instance is null);
            return null!;
        }

        return GetInstanceObject(instance);
    }

    private static unsafe GodotObject GetInstanceObject(void* instance)
    {
        var gcHandle = GCHandle<GodotObject>.FromIntPtr((nint)instance);
        var instanceObj = gcHandle.Target;

        return instanceObj;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GetArgOrDefault<[MustBeVariant] T>(MethodDefinition methodDefinition, NativeGodotVariantPtrSpan args, int index)
    {
        if (index >= args.Length)
        {
            Debug.Assert(index < methodDefinition.Parameters.Count);
            Variant? defaultValue = methodDefinition.Parameters[index].DefaultValue;
            Debug.Assert(defaultValue is not null);
            return defaultValue.Value.As<T>();
        }

        return Marshalling.ConvertFromVariant<T>(in args[index]);
    }
}
