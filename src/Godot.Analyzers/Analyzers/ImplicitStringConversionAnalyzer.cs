using System.Collections.Immutable;
using Godot.Common.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Godot.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class ImplicitStringConversionAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create([
        Descriptors.GODOT0005_AvoidImplicitStringConversion,
    ]);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterOperationAction(AnalyzeConversion, OperationKind.Conversion);
    }

    private void AnalyzeConversion(OperationAnalysisContext context)
    {
        var conversion = (IConversionOperation)context.Operation;

        if (!conversion.IsImplicit)
        {
            // The conversion is explicit, so it must be intentional.
            return;
        }

        if (conversion.Operand.Type?.SpecialType != SpecialType.System_String)
        {
            return;
        }

        var targetType = conversion.Type;
        if (targetType is null)
        {
            return;
        }

        string targetTypeName = targetType.ToDisplayString();
        if (targetTypeName != KnownTypeNames.GodotStringName && targetTypeName != KnownTypeNames.GodotNodePath)
        {
            return;
        }

        if (IsInStaticFieldOrPropertyInitializer(conversion))
        {
            // Static field/property initializers run once per type, so there's no performance concern.
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(
            Descriptors.GODOT0005_AvoidImplicitStringConversion,
            conversion.Syntax.GetLocation(),
            // Message Format parameters.
            targetType.Name
        ));
    }

    private static bool IsInStaticFieldOrPropertyInitializer(IOperation operation)
    {
        IOperation? parent = operation.Parent;

        while (parent is not null)
        {
            switch (parent)
            {
                case IFieldInitializerOperation fieldInitializer:
                    return IsStaticFieldInitializer(fieldInitializer);

                case IPropertyInitializerOperation propertyInitializer:
                    return IsStaticPropertyInitializer(propertyInitializer);
            }

            parent = parent.Parent;
        }

        return false;

        static bool IsStaticFieldInitializer(IFieldInitializerOperation fieldInitializer)
        {
            foreach (var field in fieldInitializer.InitializedFields)
            {
                if (field.IsStatic)
                {
                    return true;
                }
            }

            return false;
        }

        static bool IsStaticPropertyInitializer(IPropertyInitializerOperation propertyInitializer)
        {
            foreach (var property in propertyInitializer.InitializedProperties)
            {
                if (property.IsStatic)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
