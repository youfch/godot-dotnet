using System.CodeDom.Compiler;
using Godot.BindingsGeneration.Reflection;

namespace Godot.BindingsGeneration;

/// <summary>
/// Writes the marshalling code to convert between a managed type and
/// <see cref="KnownTypes.NativeGodotVariant"/>.
/// </summary>
internal abstract class VariantMarshallerWriter
{
    public TypeInfo MarshallableType => GetMarshallableType();

    protected abstract TypeInfo GetMarshallableType();

    /// <summary>
    /// Indicates whether the marshaller needs to cleanup afterwards.
    /// This is sometimes needed for marshallers that may allocate during
    /// <see cref="WriteSetupToVariant(IndentedTextWriter, TypeInfo, string, string)"/>
    /// or <see cref="WriteConvertToVariant(IndentedTextWriter, TypeInfo, string, string)"/>.
    /// </summary>
    public virtual bool NeedsCleanup => true;

    /// <summary>
    /// Indicates whether the marshaller needs to cleanup afterwards when used for parameters.
    /// By default, it inherits the value of <see cref="NeedsCleanup"/> but it can be overridden
    /// if the marshaller only needs to cleanup for parameters or for return values.
    /// </summary>
    public virtual bool NeedsCleanupForParameters => NeedsCleanup;

    /// <summary>
    /// Indicates whether the marshaller needs to cleanup afterwards when used for return values.
    /// By default, it inherits the value of <see cref="NeedsCleanup"/> but it can be overridden
    /// if the marshaller only needs to cleanup for parameters or for return values.
    /// </summary>
    public virtual bool NeedsCleanupForReturnValue => NeedsCleanup;

    /// <summary>
    /// Write the necessary lines to setup the conversion to unmanaged.
    /// The marshaller may create an aux variable with the name
    /// <paramref name="destination"/> if it needs it for marshalling and returns
    /// <see langword="true"/> to indicate that this variable should be used for
    /// marshalling instead of <paramref name="source"/>.
    /// </summary>
    /// <param name="writer">Writer to write the lines to.</param>
    /// <param name="type">Type that needs to be converted.</param>
    /// <param name="source">Variable name of the source.</param>
    /// <param name="destination">Variable name of the destination.</param>
    /// <returns>Whether the <paramref name="destination"/> variable was used.</returns>
    public bool WriteSetupToVariant(IndentedTextWriter writer, TypeInfo type, string source, string destination)
    {
        return WriteSetupToVariantCore(writer, type, source, destination);
    }

    /// <inheritdoc cref="WriteSetupToVariant(IndentedTextWriter, TypeInfo, string, string)"/>
    protected virtual bool WriteSetupToVariantCore(IndentedTextWriter writer, TypeInfo type, string source, string destination)
    {
        return false;
    }

    /// <summary>
    /// Write the necessary lines to setup the conversion to unmanaged.
    /// The marshaller may create an uninitialized aux variable with the name
    /// <paramref name="destination"/> if it needs it for marshalling and returns
    /// <see langword="true"/> to indicate that this variable should be used for
    /// marshalling.
    /// </summary>
    /// <param name="writer">Writer to write the lines to.</param>
    /// <param name="type">Type that needs to be converted.</param>
    /// <param name="destination">Variable name of the destination.</param>
    /// <returns>Whether the <paramref name="destination"/> variable was used.</returns>
    public bool WriteSetupToVariantUninitialized(IndentedTextWriter writer, TypeInfo type, string destination)
    {
        return WriteSetupToVariantUninitializedCore(writer, type, destination);
    }

    /// <inheritdoc cref="WriteSetupToVariantUninitialized(IndentedTextWriter, TypeInfo, string)"/>
    protected virtual bool WriteSetupToVariantUninitializedCore(IndentedTextWriter writer, TypeInfo type, string destination)
    {
        return false;
    }

    /// <summary>
    /// Write the necessary lines to convert the value stored in a variable
    /// with the name <paramref name="source"/> and store the
    /// <see cref="KnownTypes.NativeGodotVariant"/> pointer in a variable with
    /// the name <paramref name="destination"/>.
    /// </summary>
    /// <param name="writer">Writer to write the lines to.</param>
    /// <param name="type">Type that needs to be converted.</param>
    /// <param name="source">Variable name of the source.</param>
    /// <param name="destination">Variable name of the destination.</param>
    public void WriteConvertToVariant(IndentedTextWriter writer, TypeInfo type, string source, string destination)
    {
        WriteConvertToVariantCore(writer, type, source, destination);
    }

    /// <inheritdoc cref="WriteConvertToVariant(IndentedTextWriter, TypeInfo, string, string)"/>
    protected abstract void WriteConvertToVariantCore(IndentedTextWriter writer, TypeInfo type, string source, string destination);

    /// <summary>
    /// Write the necessary lines to convert the <see cref="KnownTypes.NativeGodotVariant"/>
    /// pointer stored in a variable with the name <paramref name="source"/> and store the
    /// converted value in a variable with the name <paramref name="destination"/>.
    /// </summary>
    /// <param name="writer">Writer to write the lines to.</param>
    /// <param name="type">Type that needs to be converted.</param>
    /// <param name="source">Variable name of the source.</param>
    /// <param name="destination">Variable name of the destination.</param>
    public void WriteConvertFromVariant(IndentedTextWriter writer, TypeInfo type, string source, string destination)
    {
        WriteConvertFromVariantCore(writer, type, source, destination);
    }

    /// <inheritdoc cref="WriteConvertFromVariant(IndentedTextWriter, TypeInfo, string, string)"/>
    protected abstract void WriteConvertFromVariantCore(IndentedTextWriter writer, TypeInfo type, string source, string destination);

    /// <summary>
    /// Write the necessary lines to free the <see cref="KnownTypes.NativeGodotVariant"/>
    /// pointer stored in a variable with the name <paramref name="source"/>.
    /// The pointer to free should be the one allocated by
    /// <see cref="WriteConvertToVariant(IndentedTextWriter, TypeInfo, string, string)"/>.
    /// </summary>
    /// <remarks>
    /// If this writer doesn't allocate memory in
    /// <see cref="WriteConvertToVariant(IndentedTextWriter, TypeInfo, string, string)"/>
    /// it doesn't have to free anything here and can be a no-op.
    /// </remarks>
    /// <param name="writer">Writer to write the lines to.</param>
    /// <param name="type">Type that was converted.</param>
    /// <param name="source">Variable name of the source.</param>
    public void WriteFree(IndentedTextWriter writer, TypeInfo type, string source)
    {
        WriteFreeCore(writer, type, source);
    }

    /// <inheritdoc cref="WriteFree(IndentedTextWriter, TypeInfo, string)"/>
    protected abstract void WriteFreeCore(IndentedTextWriter writer, TypeInfo type, string source);

    /// <summary>
    /// Write the necessary lines to free the <see cref="KnownTypes.NativeGodotVariant"/>
    /// pointer stored in a variable with the name <paramref name="source"/>.
    /// The pointer to free should be the one allocated by
    /// <see cref="WriteConvertToVariant(IndentedTextWriter, TypeInfo, string, string)"/>.
    /// </summary>
    /// <remarks>
    /// If this writer doesn't allocate memory in
    /// <see cref="WriteConvertToVariant(IndentedTextWriter, TypeInfo, string, string)"/>
    /// it doesn't have to free anything here and can be a no-op.
    /// </remarks>
    /// <param name="writer">Writer to write the lines to.</param>
    /// <param name="type">Type that was converted.</param>
    /// <param name="parameterName">Name of the parameter that <paramref name="source"/> corresponds to.</param>
    /// <param name="source">Variable name of the source.</param>
    public void WriteFreeForParameter(IndentedTextWriter writer, TypeInfo type, string parameterName, string source)
    {
        WriteFreeForParameterCore(writer, type, parameterName, source);
    }

    /// <inheritdoc cref="WriteFreeForParameter(IndentedTextWriter, TypeInfo, string, string)"/>
    protected virtual void WriteFreeForParameterCore(IndentedTextWriter writer, TypeInfo type, string parameterName, string source)
    {
        WriteFreeCore(writer, type, source);
    }

    /// <summary>
    /// Write the necessary lines to free the <see cref="KnownTypes.NativeGodotVariant"/>
    /// pointer stored in a variable with the name <paramref name="source"/>.
    /// The pointer to free should be the one allocated by
    /// <see cref="WriteConvertToVariant(IndentedTextWriter, TypeInfo, string, string)"/>.
    /// </summary>
    /// <remarks>
    /// If this writer doesn't allocate memory in
    /// <see cref="WriteConvertToVariant(IndentedTextWriter, TypeInfo, string, string)"/>
    /// it doesn't have to free anything here and can be a no-op.
    /// </remarks>
    /// <param name="writer">Writer to write the lines to.</param>
    /// <param name="type">Type that was converted.</param>
    /// <param name="returnVariableName">
    /// Name of the return variable that <paramref name="source"/> corresponds to.
    /// </param>
    /// <param name="source">Variable name of the source.</param>
    public void WriteFreeForReturnValue(IndentedTextWriter writer, TypeInfo type, string returnVariableName, string source)
    {
        WriteFreeForReturnValueCore(writer, type, returnVariableName, source);
    }

    /// <inheritdoc cref="WriteFreeForReturnValue(IndentedTextWriter, TypeInfo, string, string)"/>
    protected virtual void WriteFreeForReturnValueCore(IndentedTextWriter writer, TypeInfo type, string returnVariableName, string source)
    {
        WriteFreeCore(writer, type, source);
    }
}
