using System;
using System.Runtime.CompilerServices;

namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestBase : Node
{
    private int _testFailures;

    private void AssertFail(string message, string? callerName, string? callerFile, int callerLine)
    {
        _testFailures++;
        GD.Print($"FAILURE: In function {callerName} at {callerFile}:{callerLine}");
        GD.Print($" └─ {message}");
        GD.Print($"GODOT_TEST_RESULT::FAIL: {message}");
    }

    public void AssertEqual(object? expected, object? actual, [CallerMemberName] string? callerName = null, [CallerFilePath] string? callerFile = null, [CallerLineNumber] int callerLine = 0)
    {
        if (!Equals(expected, actual))
        {
            AssertFail($"Expected '{expected}' but got '{actual}'.", callerName, callerFile, callerLine);
        }
    }

    public void AssertNotEqual(object? expected, object? actual, [CallerMemberName] string? callerName = null, [CallerFilePath] string? callerFile = null, [CallerLineNumber] int callerLine = 0)
    {
        if (Equals(expected, actual))
        {
            AssertFail($"Expected '{expected}' NOT to equal '{actual}'.", callerName, callerFile, callerLine);
        }
    }

    public void AssertTrue(bool condition, [CallerMemberName] string? callerName = null, [CallerFilePath] string? callerFile = null, [CallerLineNumber] int callerLine = 0)
    {
        if (!condition)
        {
            AssertFail("Expected condition to be true, but it was false.", callerName, callerFile, callerLine);
        }
    }

    public void AssertFalse(bool condition, [CallerMemberName] string? callerName = null, [CallerFilePath] string? callerFile = null, [CallerLineNumber] int callerLine = 0)
    {
        if (condition)
        {
            AssertFail("Expected condition to be false, but it was true.", callerName, callerFile, callerLine);
        }
    }

    public void AssertThrow<TException>(Action action, [CallerMemberName] string? callerName = null, [CallerFilePath] string? callerFile = null, [CallerLineNumber] int callerLine = 0) where TException : Exception
    {
        try
        {
            action();
            AssertFail($"Expected exception of type {typeof(TException)} to be thrown, but no exception was thrown.", callerName, callerFile, callerLine);
        }
        catch (TException)
        {
            // Expected exception was thrown, test passes.
        }
        catch (Exception ex)
        {
            AssertFail($"Expected exception of type {typeof(TException)} to be thrown, but got exception of type {ex.GetType().Name}.", callerName, callerFile, callerLine);
        }
    }

    public void AssertNotThrow(Action action, [CallerMemberName] string? callerName = null, [CallerFilePath] string? callerFile = null, [CallerLineNumber] int callerLine = 0)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            AssertFail($"Expected no exception to be thrown, but got exception of type {ex.GetType()}: {ex.Message}", callerName, callerFile, callerLine);
        }
    }

    public void ExitWithStatus()
    {
        bool success = _testFailures == 0;

        if (success)
        {
            GD.Print("GODOT_TEST_RESULT::PASS: All asserts passed!");
        }

        GetTree().Quit(success ? 0 : 1);
    }
}
