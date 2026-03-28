# Godot.Templates.IntegrationTests

End-to-end tests against the [`Godot.Templates`](../../src/Godot.Templates) package, to test creating projects and items using the templates creates working code that builds and runs.

## How it works

Each test launches a new `dotnet new` process that creates a project or item, using a template from `Godot.Templates`, on a temporary directory outside this workspace. Optionally, some of the created projects are built with `dotnet build` to verify the created project compiles.
