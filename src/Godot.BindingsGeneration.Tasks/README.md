# Godot.BindingsGeneration.Tasks

MSBuild task to generate bindings for Godot contained in the [Godot.Bindings](../Godot.Bindings) package, using the [Godot.BindingsGeneration](../Godot.BindingsGeneration) library.

## Usage

Add a `UsingTask` declaration pointing at the task assembly, then invoke the `GenerateTask` in a target:

```xml
<UsingTask TaskName="Godot.BindingsGeneration.GenerateTask" AssemblyFile="path\to\Godot.BindingsGeneration.Tasks.dll" TaskFactory="TaskHostFactory" />

<Target Name="GenerateGodotBindings" BeforeTargets="BeforeCompile">
  <GenerateTask ExtensionApiPath="path\to\extension_api.json"
                ExtensionInterfacePath="path\to\gdextension_interface.h"
                OutputPath="$(MSBuildThisFileDirectory)Generated" />
  <ItemGroup>
    <Compile Include="$(MSBuildThisFileDirectory)Generated\**\*.cs" />
  </ItemGroup>
</Target>
```
