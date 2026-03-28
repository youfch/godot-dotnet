namespace Godot.Templates.IntegrationTests;

public readonly struct TemplateTestResult
{
    public string Path { get; }

    public TemplateTestResult(string path)
    {
        Path = path;
    }
}
