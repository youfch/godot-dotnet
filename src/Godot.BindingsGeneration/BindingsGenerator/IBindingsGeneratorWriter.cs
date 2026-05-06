using System.IO;

namespace Godot.BindingsGeneration;

/// <summary>
/// Factory interface for creating writers used by the bindings generator.
/// </summary>
public interface IBindingsGeneratorWriterFactory
{
    /// <summary>
    /// Indicates whether the factory can create writers for test output.
    /// If <see langword="false"/>, test output is not generated.
    /// </summary>
    public bool SupportsTestOutput { get; }

    /// <summary>
    /// Creates a new <see cref="TextWriter"/> instance.
    /// </summary>
    /// <param name="pathHint">The path hint for the generated type.</param>
    /// <param name="isTestOutput">Indicates whether the writer will be used for test output.</param>
    /// <returns>A new <see cref="TextWriter"/> instance.</returns>
    public TextWriter CreateWriter(string pathHint, bool isTestOutput);
}
