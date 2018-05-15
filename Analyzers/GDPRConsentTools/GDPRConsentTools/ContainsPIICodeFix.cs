using Microsoft.AspNetCore.Mvc.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GDPRConsentTools
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ContainsPIICodeFix)), Shared]
    public class ContainsPIICodeFix : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticDescriptors.GDPR1004_MethodAccessingPIIMustHavePurpose.Id);

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();

            var syntaxTree = await context.Document.GetSyntaxTreeAsync(context.CancellationToken);
            var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken);
            var compilation = semanticModel.Compilation;

            INamedTypeSymbol purposeTypeSymbol = compilation.GetTypeByMetadataName(TypeNames.PurposeAttribute);
            var method = semanticModel.GetDeclaredSymbol(declaration, context.CancellationToken);
            var attributes = method.GetAttributes();

            var pii = context.Diagnostics.FirstOrDefault().Properties["pii"];

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: DiagnosticDescriptors.GDPR1004_MethodAccessingPIIMustHavePurpose.Title.ToString(),
                    createChangedDocument: c => UpdateAttribute(context.Document, declaration, "", pii, c)),
                diagnostic);
        }

        private async Task<Document> UpdateAttribute(Document document, MethodDeclarationSyntax declaration, string purpose, string pii, CancellationToken cancellationToken)
        {
            purpose = purpose.ToString().Trim(new Char[] { '\\', '"' });
            pii = pii.ToString().Trim(new Char[] { '\\', '"' });

            var arguments = SyntaxFactory.ParseAttributeArgumentList($"(\"{purpose}\", \"{pii}\")");

            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var attributes = declaration.AttributeLists.Add(
                SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                    SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Purpose"))
                  .WithArgumentList(arguments)
                )).NormalizeWhitespace()
                .WithLeadingTrivia(declaration.GetLeadingTrivia()));

            return document.WithSyntaxRoot(
                root.ReplaceNode(
                    declaration,
                    declaration.WithAttributeLists(attributes)
                ));
        }
    }
}
