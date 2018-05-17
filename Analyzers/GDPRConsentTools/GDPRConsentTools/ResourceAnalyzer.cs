using Microsoft.AspNetCore.Mvc.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Linq;
using GDPR.Analyzers;
using System.Collections.Immutable;
using System;

namespace GDPRConsentTools
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ResourceAnalyzer : ResourceAnalyzerBase
    {
        public ResourceAnalyzer()
            : base(DiagnosticDescriptors.GDPR1002_StorePurposeInPolicies)
        {
        }

        protected override void InitializeWorker(ResourceAnalyzerContext analyzerContext)
        {
            analyzerContext.Context.RegisterSyntaxNodeAction(context =>
            {
                var attribute = (AttributeSyntax)context.Node;

                if (attribute.ArgumentList == null)
                {
                    return;
                }

                if (!attribute.DescendantNodes().OfType<AttributeArgumentSyntax>().Any())
                {
                    return;
                }

                SymbolInfo symbolInfo = context.SemanticModel.GetSymbolInfo(attribute);
                ISymbol symbol = symbolInfo.Symbol;

                if (symbol == null)
                {
                    return;
                }

                if (!analyzerContext.IsPurpose(symbol))
                {
                    return;
                }

                if (!attribute.ArgumentList.DescendantNodes().OfType<AttributeArgumentSyntax>().Any())
                {
                    return;
                }

                if (attribute.ArgumentList.Arguments.Count() >= 2)
                {
                    var builder = ImmutableDictionary.CreateBuilder<string, string>();

                    var purpose = attribute.ArgumentList.Arguments[0].ToString().Trim(new Char[] { '\\', '"' });
                    var pii = attribute.ArgumentList.Arguments[1].ToString().Trim(new Char[] { '\\', '"' });

                    builder.Add("purpose", purpose);
                    builder.Add("pii", pii);

                    var location = attribute.GetLocation();
                    context.ReportDiagnostic(Diagnostic.Create(this.SupportedDiagnostic,
                        location, properties: builder.ToImmutable()));
                }

            }, SyntaxKind.Attribute);
        }
    }
}
