using Godot;

namespace GodotProjectName;

[GodotClass]
public partial class MainScene : Node
{
    protected override void _Ready()
    {
        GD.Print("Hello, World!");
    }
}
