using System.CodeDom.Compiler;
using Godot.BindingsGeneration.ApiDump;
using Godot.BindingsGeneration.Reflection;

namespace Godot.BindingsGeneration;

internal sealed class CallMethodBindVararg : VarargCallMethodBody<VarargCallMethodBodyContext>
{
    private readonly MethodInfo _method;
    private readonly GodotMethodInfo _engineMethod;

    public CallMethodBindVararg(MethodInfo method, GodotMethodInfo engineMethod, TypeDB typeDB) : base(typeDB)
    {
        _method = method;
        _engineMethod = engineMethod;
    }

    protected override VarargCallMethodBodyContext CreateVarargCallContext(MethodBase owner)
    {
        TypeInfo? returnType = owner is MethodInfo method ? method.ReturnType : null;

        return new VarargCallMethodBodyContext()
        {
            IsStatic = owner.IsStatic,
            Parameters = owner.Parameters,
            ReturnType = returnType,
            MarshalReturnTypeAsPtr = false,
        };
    }

    protected override void SetupInstanceParameter(VarargCallMethodBodyContext context, IndentedTextWriter writer)
    {
        writer.WriteLine("void* __instance = (void*)GetNativePtr(this);");
    }

    protected override void RetrieveMethodBind(VarargCallMethodBodyContext context, IndentedTextWriter writer)
    {
        writer.WriteLine($"global::Godot.NativeInterop.MethodBind.GetAndCacheMethodBind(ref _{_method.Name}_MethodBind, NativeName, MethodName.{_method.Name}, {_engineMethod.Hash}L);");
    }

    protected override void InvokeMethodBind(VarargCallMethodBodyContext context, IndentedTextWriter writer)
    {
        string instanceVariable = !context.IsStatic ? context.InstanceVariableName : "null";
        string returnVariable;
        if (context.ReturnType is not null)
        {
            returnVariable = $"&{context.ReturnVariableName}";
            if (context.ReturnType != KnownTypes.NativeGodotVariant)
            {
                returnVariable += "Var";
            }
        }
        else
        {
            // The method has no return type, but the call still needs to pass a valid pointer for the return value.
            writer.WriteLine($"global::Godot.NativeInterop.NativeGodotVariant __dummyRet = default;");
            returnVariable = "&__dummyRet";
        }

        writer.WriteLine($"global::Godot.NativeInterop.GDExtensionCallError __callError;");

        writer.WriteLine($"global::Godot.Bridge.GodotBridge.GDExtensionInterface.object_method_bind_call(_{_method.Name}_MethodBind, {instanceVariable}, {context.ArgsVariableName}, {context.ArgsCountVariableName}, {returnVariable}, &__callError);");

        writer.WriteLine($"global::Godot.NativeInterop.MethodBind.DebugCheckCallError(MethodName.{_method.Name}.NativeValue.DangerousSelfRef, {instanceVariable}, new global::Godot.NativeInterop.NativeGodotVariantPtrSpan({context.ArgsVariableName}, {context.ArgsCountVariableName}), __callError);");
    }
}
