using System;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Frameworks;
using Xunit.Abstractions;

namespace Godot.Templates.IntegrationTests;

[Collection(nameof(TemplatesCollectionFixture))]
public abstract class IntegrationTestBase
{
    private readonly TemplatesFixture _templates;
    private readonly ITestOutputHelper _output;

    protected IntegrationTestBase(TemplatesFixture templates, ITestOutputHelper output)
    {
        _templates = templates;
        _output = output;
    }

    public sealed class Test : TemplateRunner
    {
        public Task<TemplateTestResult> RunAsync(params string[] dotnetNewArgs)
        {
            return RunAsync(dotnetNewArgs, CancellationToken.None);
        }

        public new async Task<TemplateTestResult> RunAsync(string[] dotnetNewArgs, CancellationToken cancellationToken = default)
        {
            await base.RunAsync(dotnetNewArgs, cancellationToken);
            return new TemplateTestResult(TemplateOutputDir.FullName);
        }
    }

    protected Task<TemplateTestResult> Verify(params string[] dotnetNewArgs)
    {
        return MakeVerifier().RunAsync(dotnetNewArgs);
    }

    protected Test MakeVerifier()
    {
        return new Test()
        {
            CustomHive = _templates.CustomHive,
            Output = _output,
        };
    }
}
