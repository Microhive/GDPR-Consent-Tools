using GDPR.Analyzers;
using Microsoft.AspNetCore.Mvc.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Linq;

namespace GDPRConsentTools
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ContainsPII : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(DiagnosticDescriptors.GDPR1004_MethodAccessingPIIMustHavePurpose);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(syntaxContext =>
            {
                // Find methods without purpose attribute
                var methodSyntax = (MethodDeclarationSyntax)syntaxContext.Node;
                var method = syntaxContext.SemanticModel.GetDeclaredSymbol(methodSyntax, syntaxContext.CancellationToken);

                INamedTypeSymbol purposeTypeSymbol = syntaxContext.Compilation.GetTypeByMetadataName(TypeNames.PurposeAttribute);

                if (purposeTypeSymbol == null)
                {
                    return;
                }

                if (method.HasAttribute(purposeTypeSymbol))
                {
                    return;
                }

                // Get all variable declaration
                INamedTypeSymbol piiTypeSymbol = syntaxContext.Compilation.GetTypeByMetadataName(TypeNames.PIIAttribute);
                if (piiTypeSymbol == null)
                {
                    return;
                }

                var variableDeclarations = methodSyntax
                    .DescendantNodes()
                    .OfType<LocalDeclarationStatementSyntax>();

                var pii = new List<string>();
                foreach (var variableDeclaration in variableDeclarations)
                {
                    var symbolInfo = syntaxContext.SemanticModel.GetSymbolInfo(variableDeclaration.Declaration.Type, syntaxContext.CancellationToken);
                    var typeSymbol = symbolInfo.Symbol;

                    if (typeSymbol == null)
                    {
                        continue;
                    }

                    // If generic named type<T>, get type of T.
                    INamedTypeSymbol namedType = (INamedTypeSymbol)typeSymbol;
                    ITypeSymbol type = namedType.TypeArguments.FirstOrDefault();
                    var symbol = type ?? typeSymbol;

                    if (symbol.HasAttribute(piiTypeSymbol))
                    {
                        foreach (var declaredAttribute in symbol.GetAttributes())
                        {
                            if (declaredAttribute.AttributeClass == piiTypeSymbol)
                            {
                                var symanticModel = syntaxContext.Compilation.GetSemanticModel(declaredAttribute.ApplicationSyntaxReference.SyntaxTree);
                                var attribute = declaredAttribute.ApplicationSyntaxReference.GetSyntax(syntaxContext.CancellationToken);
                                var arguments = attribute.DescendantNodes().OfType<AttributeArgumentSyntax>();

                                foreach (var argument in arguments)
                                {
                                    pii.Add(argument.ToString().Trim(new Char[] { ' ', '\\', '"' }));
                                }
                            }
                        }
                    }
                }

                if (pii.Count() == 0)
                {
                    return;
                }

                var builder = ImmutableDictionary.CreateBuilder<string, string>();
                builder["pii"] = string.Join(",", pii);
                var properies = builder.ToImmutable();

                var location = methodSyntax.Identifier.GetLocation();
                syntaxContext.ReportDiagnostic(Diagnostic.Create(this.SupportedDiagnostics.FirstOrDefault(),
                            location, properties: properies));

            }, SyntaxKind.MethodDeclaration);
        }
    }
}