using System;

namespace Godot.Bindings.IntegrationTests;

public readonly struct GodotTestResult
{
    public bool Success { get; }

    public string Message { get; }

    public GodotTestResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static GodotTestResult Parse(string resultLine)
    {
        if (resultLine.StartsWith(GodotRunner.ResultPassPrefix, StringComparison.Ordinal))
        {
            return new GodotTestResult(true, string.Empty);
        }

        string message = resultLine.Length > GodotRunner.ResultFailPrefix.Length
            ? resultLine[GodotRunner.ResultFailPrefix.Length..].Trim()
            : string.Empty;

        return new GodotTestResult(false, message);
    }
}
