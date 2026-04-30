namespace Godot.BindingsGeneration.Logging;

/// <summary>
/// Logger that does nothing and discards all messages.
/// </summary>
internal sealed class NullLogger : ILogger
{
    public static NullLogger Instance { get; } = new();

    private NullLogger() { }

    public void Log(LogLevel logLevel, string message) { }
}
