using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.AspNetCore.Mvc.Analyzers;

namespace GDPRConsentTools
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ResourceCodeFixProvider)), Shared]
    public class ResourceCodeFixProvider : CodeFixProvider
    {
        private const string title = "Add Purpose to Container";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.GDPR1002_StorePurposeInPolicies.Id); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var argument = context.Diagnostics.FirstOrDefault().Properties;
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var project = context.Document.Project;
            var solution = await project.GetCompilationAsync(context.CancellationToken).ConfigureAwait(false);

            if (project == null)
            {
                return;
            }

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var documents = project.Documents;
            var document = context.Document.Project.Solution.Projects.SelectMany(x => x.Documents).FirstOrDefault(x => x.Name == "Policies.cs"); //documents.FirstOrDefault(x => x.Name == "GDPR.cs");

            if (document == null)
            {
                return;
            }

            var treeAsync = await document.GetSyntaxTreeAsync();
            var classes = treeAsync.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
            var semanticModel = await document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);

            if (semanticModel == null || classes == null)
            {
                return;
            }

            var symbols = classes.Select(x => semanticModel.GetDeclaredSymbol(x));
            if (!symbols.Any())
            {
                return;
            }

            INamedTypeSymbol typeNamePII = semanticModel.Compilation.GetTypeByMetadataName(TypeNames.PIIAttribute);
            INamedTypeSymbol typeNameInterface = semanticModel.Compilation.GetTypeByMetadataName(TypeNames.IGDPRPolicies);
            INamedTypeSymbol typeNameAttribute = semanticModel.Compilation.GetTypeByMetadataName(TypeNames.GDPRPoliciesAttribute);
            var gdprPolicyClass = symbols.FirstOrDefault(x => x.Interfaces.Contains(typeNameInterface) && x.HasAttribute(typeNameAttribute));
            if (gdprPolicyClass == null)
            {
                return;
            }

            var names = gdprPolicyClass.GetMembers().Cast<IMethodSymbol>();
            var types = gdprPolicyClass.GetTypeMembers();

            var method = names.FirstOrDefault(x => x.MetadataName == "GeneratePolicies");

            if (method == null)
            {
                return;
            }

            var syntaxReference = method.DeclaringSyntaxReferences.First();
            var node = await syntaxReference.GetSyntaxAsync();
            var block = node.ChildNodes().OfType<BlockSyntax>().FirstOrDefault();

            var oldSpan = block.Span;
            var statement = ImmutableArray.Create(SyntaxFactory.ParseStatement($"list.Add(new Policy(purpose: \"{argument.GetValueOrDefault("purpose")}\", pii: \"{argument.GetValueOrDefault("pii")}\"));\n")).ToArray();
            var newBlock = block.AddStatements(statement);

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: title,
                    createChangedDocument: c => AddDocument(document, oldSpan, newBlock, c),
                    equivalenceKey: title),
                diagnostic);
        }

        private async Task<Document> AddDocument(Document document, TextSpan span, BlockSyntax block1, CancellationToken cancellationToken)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken);
            var node = syntaxRoot.FindNode(span);
            var solution = document.Project.Solution;
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);

            var newSyntaxRoot = syntaxRoot.ReplaceNode(node, block1).NormalizeWhitespace();
            var newDoc = document.WithSyntaxRoot(newSyntaxRoot);
            return newDoc;
        }
    }
}
