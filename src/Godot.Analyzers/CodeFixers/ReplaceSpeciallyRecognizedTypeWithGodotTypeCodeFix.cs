using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Godot.Common.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Godot.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ReplaceSpeciallyRecognizedTypeWithGodotTypeCodeFix))]
internal sealed class ReplaceSpeciallyRecognizedTypeWithGodotTypeCodeFix : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create([
        Descriptors.GODOT0003_MarhsallingRequiresCopying.Id,
    ]);

    public override FixAllProvider? GetFixAllProvider() =>
        WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return;
        }

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var syntaxNode = root
            .FindToken(diagnosticSpan.Start).Parent?
            .AncestorsAndSelf()
            .OfType<TypeSyntax>()
            .FirstOrDefault();

        if (syntaxNode is null)
        {
            return;
        }

        if (syntaxNode.Parent is ArrayTypeSyntax arrayTypeSyntax)
        {
            // If the type syntax is part of an array type, we want to replace the entire array type.
            syntaxNode = arrayTypeSyntax;
        }

        var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
        if (semanticModel is null)
        {
            return;
        }

        var typeSymbol = semanticModel.GetTypeInfo(syntaxNode, context.CancellationToken).Type;
        if (typeSymbol is null)
        {
            return;
        }

        var equivalentTypes = GetEquivalentTypes(semanticModel.Compilation, typeSymbol);
        if (equivalentTypes.Count == 0)
        {
            // No suitable equivalent types found.
            return;
        }

        var annotation = new SyntaxAnnotation();
        var annotatedNode = syntaxNode.WithAdditionalAnnotations(annotation);
        var newRoot = root.ReplaceNode(syntaxNode, annotatedNode);
        var newDocument = context.Document.WithSyntaxRoot(newRoot);

        foreach (var equivalentTypeOption in equivalentTypes)
        {
            var codeAction = CodeAction.Create(
                title: SR.FormatGODOT0003_ReplaceSpeciallyRecognizedTypeWithGodotType_CodeFix(equivalentTypeOption.TypeName),
                equivalenceKey: nameof(ReplaceSpeciallyRecognizedTypeWithGodotTypeCodeFix),
                createChangedDocument: cancellationToken => ApplyFix(newDocument, annotation, equivalentTypeOption, cancellationToken));

            context.RegisterCodeFix(codeAction, diagnostic);
        }
    }

    private static async Task<Document> ApplyFix(Document document, SyntaxAnnotation annotation, TypeReplaceOption newTypeOption, CancellationToken cancellationToken = default)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        foreach (string newTypeNamespace in newTypeOption.Namespaces)
        {
            root = SyntaxUtils.AddUsingDirective(root, newTypeNamespace);
        }

        // Since AddUsingDirective can modify the syntax tree, once root is modified
        // the previous syntax node we had won't match anymore. We need to look in the
        // modified root to find the equivalent syntax node.
        var syntaxNode = root.GetAnnotatedNodes(annotation).FirstOrDefault();
        if (syntaxNode is null)
        {
            return document;
        }

        var typeSyntax = SyntaxFactory.ParseTypeName(newTypeOption.TypeName);
        var newTypeSyntax = typeSyntax.WithTriviaFrom(syntaxNode);

        var newRoot = root.ReplaceNode(syntaxNode, newTypeSyntax);
        return document.WithSyntaxRoot(newRoot);
    }

    private record class TypeReplaceOption(string TypeName, HashSet<string> Namespaces);

    private static List<TypeReplaceOption> GetEquivalentTypes(Compilation compilation, ITypeSymbol originalTypeSymbol)
    {
        List<TypeReplaceOption> equivalentTypes = [];

        if (originalTypeSymbol is IArrayTypeSymbol arrayTypeSymbol)
        {
            var elementTypeSymbol = arrayTypeSymbol.ElementType;

            if (Marshalling.TryGetPackedArrayType(originalTypeSymbol, elementTypeSymbol, out _, out string? packedArrayTypeFullName))
            {
                AddPackedArray(packedArrayTypeFullName);
            }

            AddGenericGodotArray(elementTypeSymbol);
        }

        if (originalTypeSymbol.TypeKind is TypeKind.Class or TypeKind.Struct or TypeKind.Interface)
        {
            string typeName = originalTypeSymbol.FullQualifiedNameOmitGlobalWithoutGenericTypeArguments();

            switch (typeName)
            {
                case KnownTypeNames.SystemCollectionsGenericList:
                case KnownTypeNames.SystemCollectionsGenericIReadOnlyList:
                case KnownTypeNames.SystemCollectionsGenericIList:
                case KnownTypeNames.SystemCollectionsGenericIEnumerable:
                case KnownTypeNames.SystemCollectionsGenericIReadOnlyCollection:
                case KnownTypeNames.SystemCollectionsGenericICollection:
                case KnownTypeNames.SystemCollectionsObjectModelCollection:
                case KnownTypeNames.SystemCollectionsObjectModelReadOnlyCollection:
                case KnownTypeNames.SystemCollectionsImmutableImmutableArray:
                {
                    if (!Marshalling.TryGetArrayLikeElementType(compilation, originalTypeSymbol, out var elementTypeSymbol))
                    {
                        break;
                    }

                    if (Marshalling.TryGetPackedArrayType(originalTypeSymbol, elementTypeSymbol, out _, out string? packedArrayTypeFullName))
                    {
                        AddPackedArray(packedArrayTypeFullName);
                    }

                    AddGenericGodotArray(elementTypeSymbol);
                    break;
                }

                case KnownTypeNames.SystemCollectionsGenericDictionary:
                case KnownTypeNames.SystemCollectionsGenericIReadOnlyDictionary:
                case KnownTypeNames.SystemCollectionsGenericIDictionary:
                case KnownTypeNames.SystemCollectionsImmutableImmutableDictionary:
                {
                    if (!Marshalling.TryGetDictionaryLikeKeyValueTypes(compilation, originalTypeSymbol, out var keyTypeSymbol, out var valueTypeSymbol))
                    {
                        break;
                    }

                    AddGenericGodotDictionary(keyTypeSymbol, valueTypeSymbol);
                    break;
                }
            }
        }

        return equivalentTypes;

        void AddPackedArray(string packedArrayTypeFullName)
        {
            string packedArrayTypeName = packedArrayTypeFullName.Substring(packedArrayTypeFullName.LastIndexOf('.') + 1);
            equivalentTypes.Add(new TypeReplaceOption(packedArrayTypeName, [
                "Godot.Collections",
            ]));
        }

        void AddGenericGodotArray(ITypeSymbol elementTypeSymbol)
        {
            HashSet<string> namespaces = ["Godot.Collections"];
            string elementTypeFullName = GetSimpleTypeName(elementTypeSymbol, namespaces);

            equivalentTypes.Add(new TypeReplaceOption($"GodotArray<{elementTypeFullName}>", namespaces));
        }

        void AddGenericGodotDictionary(ITypeSymbol keyTypeSymbol, ITypeSymbol valueTypeSymbol, string beforeTypeName = "")
        {
            HashSet<string> namespaces = ["Godot.Collections"];
            string keyElementTypeName = GetSimpleTypeName(keyTypeSymbol, namespaces);
            string valueElementTypeName = GetSimpleTypeName(valueTypeSymbol, namespaces);

            equivalentTypes.Add(new TypeReplaceOption($"GodotDictionary<{keyElementTypeName}, {valueElementTypeName}>", namespaces));
        }

        string GetSimpleTypeName(ITypeSymbol typeSymbol, HashSet<string> namespaces)
        {
            string fullTypeName = typeSymbol.FullQualifiedNameOmitGlobal();
            if (!fullTypeName.Contains('.'))
            {
                // If the fully-qualified name results in a simple name, it's likely a keyword.
                // Use this as the name and don't add a using directive for the namespace.
                return fullTypeName;
            }

            // Otherwise, add the containing namespace and use the simple name.
            namespaces.Add(typeSymbol.ContainingNamespace.FullQualifiedNameOmitGlobal());
            return typeSymbol.Name;
        }
    }
}
