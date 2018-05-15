// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace GDPR.Analyzers
{
    public abstract class PIIAnalyzerBase : DiagnosticAnalyzer
    {
        public PIIAnalyzerBase(DiagnosticDescriptor diagnostic)
        {
            SupportedDiagnostics = ImmutableArray.Create(diagnostic);
        }

        protected DiagnosticDescriptor SupportedDiagnostic => SupportedDiagnostics[0];

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }

        public sealed override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(compilationContext =>
            {
                var analyzerContext = new PIIAnalyzerContext(compilationContext);

                // Only do work if SensitiveAttribute is defined.
                if (analyzerContext.SensitiveAttribute == null)
                {
                    return;
                }
                
                InitializeWorker(analyzerContext);
            });
        }

        protected abstract void InitializeWorker(PIIAnalyzerContext analyzerContext);
    }
}
