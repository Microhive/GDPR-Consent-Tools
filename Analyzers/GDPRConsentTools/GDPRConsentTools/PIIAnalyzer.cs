// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Linq;
using GDPR.Analyzers;

namespace Analyzer1
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PIIAnalyzer : PIIAnalyzerBase
    {
        public PIIAnalyzer()
            : base(DiagnosticDescriptors.GDPR1000_ClassMustBeMarkedSensitive)
        {
        }

        protected override void InitializeWorker(PIIAnalyzerContext analyzerContext)
        {
            analyzerContext.Context.RegisterSyntaxNodeAction(context =>
            {
                var propertySyntax = (PropertyDeclarationSyntax)context.Node;
                var property = context.SemanticModel.GetDeclaredSymbol(propertySyntax, context.CancellationToken);

                if (!analyzerContext.IsPropertySensitive(property))
                {
                    return;
                }

                if (analyzerContext.IsClassSensitive(property))
                {
                    return;
                }

                var nodeSyntax = propertySyntax.Parent;
                while (context.SemanticModel.GetDeclaredSymbol(nodeSyntax, context.CancellationToken).Kind != SymbolKind.NamedType)
                {
                    nodeSyntax = nodeSyntax.Parent;
                }

                var location = nodeSyntax.GetLocation();
                context.ReportDiagnostic(Diagnostic.Create(
                    SupportedDiagnostic,
                    location));

            }, SyntaxKind.PropertyDeclaration);
        }
    }
}
