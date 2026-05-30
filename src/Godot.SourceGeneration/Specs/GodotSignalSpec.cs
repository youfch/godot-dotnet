using System;
using Microsoft.CodeAnalysis;

namespace Godot.SourceGeneration;

/// <summary>
/// Describes a Godot signal specification.
/// </summary>
internal readonly record struct GodotSignalSpec : IEquatable<GodotSignalSpec>
{
    /// <summary>
    /// Name of the delegate's symbol.
    /// This is the real name of the delegate in the source code.
    /// </summary>
    public required string SymbolName { get; init; }

    /// <summary>
    /// Name of the event's symbol that will be generated.
    /// This matches the name of the delegate without the 'EventHandler' suffix.
    /// </summary>
    public required string EventSymbolName { get; init; }

    /// <summary>
    /// Declared accessibility of the delegate's symbol.
    /// </summary>
    public required Accessibility SymbolDeclaredAccessibility { get; init; }

    /// <summary>
    /// Accessibility for the generated EmitSignal method for this signal.
    /// </summary>
    public required Accessibility EmitSignalMethodAccessibility { get; init; }

    /// <summary>
    /// Describes the parameters of the delegate that defines the signal.
    /// </summary>
    public EquatableArray<GodotPropertySpec> Parameters { get; init; }

    /// <summary>
    /// Name specified in the <c>[Signal]</c> attribute for this signal,
    /// or <see langword="null"/> if a name was not specified.
    /// If unspecified the name of the signal will be <see cref="SymbolName"/>.
    /// </summary>
    public string? NameOverride { get; init; }
}
