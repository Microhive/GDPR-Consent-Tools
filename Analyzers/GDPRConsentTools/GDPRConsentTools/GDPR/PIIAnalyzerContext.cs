// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace GDPR.Analyzers
{
    public class PIIAnalyzerContext
    {
#pragma warning disable RS1012 // Start action has no registered actions.
        public PIIAnalyzerContext(CompilationStartAnalysisContext context)
#pragma warning restore RS1012 // Start action has no registered actions.
        {
            Context = context;
            SensitiveAttribute = Context.Compilation.GetTypeByMetadataName(TypeNames.PIIAttribute);
        }

        public CompilationStartAnalysisContext Context { get; }

        public INamedTypeSymbol SensitiveAttribute { get; }

        private INamedTypeSymbol _systemThreadingTask;

        public INamedTypeSymbol SystemThreadingTask => GetType(TypeNames.Task, ref _systemThreadingTask);

        private INamedTypeSymbol _systemThreadingTaskOfT;

        public INamedTypeSymbol SystemThreadingTaskOfT => GetType(TypeNames.TaskOfT, ref _systemThreadingTaskOfT);

        public INamedTypeSymbol _hasPIIAttribute;

        public INamedTypeSymbol HasSensitiveAttribute => GetType(TypeNames.PIIAttribute, ref _hasPIIAttribute);

        private INamedTypeSymbol GetType(string name, ref INamedTypeSymbol cache) =>
            cache = cache ?? Context.Compilation.GetTypeByMetadataName(name);
        
        public bool IsClassSensitive(IPropertySymbol symbol)
        {
            return
                symbol.ContainingType.HasAttribute(SensitiveAttribute, inherit: true) &&
                symbol.DeclaredAccessibility == Accessibility.Public &&
                !symbol.IsAbstract &&
                !symbol.IsStatic;
        }

        public bool IsPropertySensitive(IPropertySymbol symbol)
        {
            return
                symbol.HasAttribute(SensitiveAttribute) &&
                symbol.DeclaredAccessibility == Accessibility.Public;
        }
    }
}
