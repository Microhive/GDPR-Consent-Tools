// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace GDPR.Analyzers
{
    public class ResourceAnalyzerContext
    {
#pragma warning disable RS1012 // Start action has no registered actions.
        public ResourceAnalyzerContext(CompilationStartAnalysisContext context)
#pragma warning restore RS1012 // Start action has no registered actions.
        {
            Context = context;
            PurposeAttribute = Context.Compilation.GetTypeByMetadataName(TypeNames.PurposeAttribute);
        }

        public CompilationStartAnalysisContext Context { get; }

        public INamedTypeSymbol PurposeAttribute { get; }

        private INamedTypeSymbol _systemThreadingTask;

        public INamedTypeSymbol SystemThreadingTask => GetType(TypeNames.Task, ref _systemThreadingTask);

        private INamedTypeSymbol _systemThreadingTaskOfT;

        public INamedTypeSymbol SystemThreadingTaskOfT => GetType(TypeNames.TaskOfT, ref _systemThreadingTaskOfT);

        public INamedTypeSymbol _IsPurposeAttribute;

        public INamedTypeSymbol IsPurposeAttribute => GetType(TypeNames.PurposeAttribute, ref _IsPurposeAttribute);

        private INamedTypeSymbol GetType(string name, ref INamedTypeSymbol cache) =>
            cache = cache ?? Context.Compilation.GetTypeByMetadataName(name);

        public bool IsPurpose(ISymbol attribute)
        {
            return attribute.ContainingType == PurposeAttribute;
        }
    }
}
