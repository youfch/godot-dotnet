# Godot.Bindings.IntegrationTests

End-to-end tests against the Godot engine, to test various features and APIs that require interop.

## How it works

Each test launches a new process that runs the Godot engine against the [`Godot.Bindings.IntegrationTests.TestGame`](../Godot.Bindings.IntegrationTests.TestGame) project, a specific scene, and some some arguments. The tests are designed to print various messages to stdout that are collected by the test runner in order to determine whether the test was successful and what messages to show for failure.

The Godot binary used to run the tests will be downloaded automatically, if it doesn't already exist, and placed in the `.godot` directory at the root of this repository. A custom Godot binary can be used by specifying the path in the `GODOT_PATH` environment variable.

## Writing tests

Test scenes can be driven by GDScript or C#:

- When GDScript is the driver, the root node has a GDScript attached that inherits from `res://TestBase.gd` with the assert methods. The test is implemented in the `_ready()` method and must end with `exit_with_status()`.
- When C# is the driver, the root node type is a C# type that inherits from `TestBase` with the assert methods. The test is implemented in the `_Ready()` method and must end with `ExitWithStatus()`.

Tests have a default timeout, in case the engine hangs or the tests don't call `exit_with_status()` or `ExitWithStatus()`. If the timeout is reached, the test is considered failed.
