using System;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Templates.IntegrationTests;

public sealed class TemplatesFixture : IAsyncLifetime
{
    public TemplatesCustomHive CustomHive { get; } = new();

    public async Task InitializeAsync()
    {
        await CustomHive.EnsureInstalledAsync(new ConsoleOutputHelper());
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private sealed class ConsoleOutputHelper : ITestOutputHelper
    {
        public void WriteLine(string message) => Console.WriteLine(message);
        public void WriteLine(string format, params object[] args) => Console.WriteLine(format, args);
    }
}
