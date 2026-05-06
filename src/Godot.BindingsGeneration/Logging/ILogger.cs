namespace Godot.BindingsGeneration.Logging;

/// <summary>
/// Logs messages during the bindings generation process.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Logs a message with the specified severity level.
    /// </summary>
    /// <param name="logLevel">The severity level of the log message.</param>
    /// <param name="message">The message to log.</param>
    public void Log(LogLevel logLevel, string message);
}
