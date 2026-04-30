using System.CodeDom.Compiler;
using Godot.BindingsGeneration.Reflection;

namespace Godot.BindingsGeneration.Marshallers;

internal sealed class IntegerVariantMarshallerWriter : VariantMarshallerWriter
{
    public static IntegerVariantMarshallerWriter Instance { get; } = new();

    public override bool NeedsCleanup => false;

    private IntegerVariantMarshallerWriter() { }

    protected override TypeInfo GetMarshallableType()
    {
        return KnownTypes.SystemInt64;
    }

    protected override bool WriteSetupToVariantCore(IndentedTextWriter writer, TypeInfo type, string source, string destination)
    {
        writer.Write($"global::Godot.NativeInterop.NativeGodotVariant {destination} = global::Godot.NativeInterop.NativeGodotVariant.CreateFromInt(");
        if (type != KnownTypes.SystemInt64)
        {
            writer.Write($"(long)({source})");
        }
        else
        {
            writer.Write(source);
        }
        writer.WriteLine(");");

        return true;
    }

    protected override void WriteConvertToVariantCore(IndentedTextWriter writer, TypeInfo type, string source, string destination)
    {
        writer.WriteLine($"{destination} = {source}.GetUnsafeAddress();");
    }

    protected override void WriteConvertFromVariantCore(IndentedTextWriter writer, TypeInfo type, string source, string destination)
    {
        writer.Write($"{destination} = global::Godot.NativeInterop.NativeGodotVariant.ConvertToInt(");
        if (type != KnownTypes.SystemInt64)
        {
            writer.Write($"({type.FullNameWithGlobal})(*{source})");
        }
        else
        {
            writer.Write($"*{source}");
        }
        writer.WriteLine(");");
    }

    protected override void WriteFreeCore(IndentedTextWriter writer, TypeInfo type, string source)
    {
        // Nothing to free.
    }
}
