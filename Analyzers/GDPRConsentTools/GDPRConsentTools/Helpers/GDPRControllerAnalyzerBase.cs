// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.AspNetCore.Mvc.Analyzers
{
    public abstract class GDPRControllerAnalyzerBase : DiagnosticAnalyzer
    {
        public GDPRControllerAnalyzerBase(DiagnosticDescriptor diagnostic)
        {
            SupportedDiagnostics = ImmutableArray.Create(diagnostic);
        }

        protected DiagnosticDescriptor SupportedDiagnostic => SupportedDiagnostics[0];

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }

        public sealed override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(compilationContext =>
            {
                var analyzerContext = new GDPRControllerAnalyzerContext(compilationContext);

                // Only do work if ControllerAttribute is defined.
                if (analyzerContext.ControllerAttribute == null)
                {
                    return;
                }

                InitializeWorker(analyzerContext);
            });
        }

        protected abstract void InitializeWorker(GDPRControllerAnalyzerContext analyzerContext);
    }
}
