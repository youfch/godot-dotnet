using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Godot.Bindings.IntegrationTests;

[Collection(nameof(GodotBinaryCollectionFixture))]
public abstract class IntegrationTestBase
{
    private readonly GodotBinaryFixture _godot;
    private readonly ITestOutputHelper _output;

    protected IntegrationTestBase(GodotBinaryFixture godot, ITestOutputHelper output)
    {
        _godot = godot;
        _output = output;
    }

    public sealed class Test : GodotRunner
    {
        public Task RunAsync(string scene, params string[] sceneArgs)
        {
            return RunAsync(scene, sceneArgs, CancellationToken.None);
        }

        public new async Task RunAsync(string scene, string[]? sceneArgs = null, CancellationToken cancellationToken = default)
        {
            List<GodotTestResult> results = await base.RunAsync(scene, sceneArgs ?? [], cancellationToken);

            List<Exception> exceptions = [];
            foreach (var result in results)
            {
                if (!result.Success)
                {
                    exceptions.Add(new InvalidOperationException(result.Message));
                }
            }

            switch (exceptions.Count)
            {
                case 1:
                    throw exceptions[0];
                case > 0:
                    throw new AggregateException(exceptions);
            }
        }
    }

    protected Task Verify(string scene, params string[] sceneArgs)
    {
        return MakeVerifier().RunAsync(scene, sceneArgs);
    }

    protected Test MakeVerifier()
    {
        return new Test()
        {
            GodotBinary = _godot.GodotBinaryPath,
            ProjectDir = _godot.TestGamePath,
            Output = _output,
        };
    }
}
