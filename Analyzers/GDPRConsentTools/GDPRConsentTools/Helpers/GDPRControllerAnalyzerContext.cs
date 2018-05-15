// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.AspNetCore.Mvc.Analyzers
{
    public class GDPRControllerAnalyzerContext
    {
#pragma warning disable RS1012 // Start action has no registered actions.
        public GDPRControllerAnalyzerContext(CompilationStartAnalysisContext context)
#pragma warning restore RS1012 // Start action has no registered actions.
        {
            Context = context;
            ControllerAttribute = Context.Compilation.GetTypeByMetadataName(TypeNames.ControllerAttribute);
            SensitiveAttribute = Context.Compilation.GetTypeByMetadataName(TypeNames.PIIAttribute);
        }

        public CompilationStartAnalysisContext Context { get; }

        public INamedTypeSymbol ControllerAttribute { get;  }
        public INamedTypeSymbol SensitiveAttribute { get; }

        private INamedTypeSymbol _systemThreadingTask;
        public INamedTypeSymbol SystemThreadingTask => GetType(TypeNames.Task, ref _systemThreadingTask);

        private INamedTypeSymbol _systemThreadingTaskOfT;
        public INamedTypeSymbol SystemThreadingTaskOfT => GetType(TypeNames.TaskOfT, ref _systemThreadingTaskOfT);

        public INamedTypeSymbol _nonActionAttribute;
        public INamedTypeSymbol NonActionAttribute => GetType(TypeNames.NonActionAttribute, ref _nonActionAttribute);

        public INamedTypeSymbol _hasSensitiveAttribute;
        public INamedTypeSymbol HasSensitiveAttribute => GetType(TypeNames.PIIAttribute, ref _hasSensitiveAttribute);

        private INamedTypeSymbol GetType(string name, ref INamedTypeSymbol cache) =>
            cache = cache ?? Context.Compilation.GetTypeByMetadataName(name);

        public bool IsControllerAction(IMethodSymbol method)
        {
            return
                method.ContainingType.HasAttribute(ControllerAttribute, inherit: true) &&
                method.DeclaredAccessibility == Accessibility.Public &&
                method.MethodKind == MethodKind.Ordinary &&
                !method.IsGenericMethod &&
                !method.IsAbstract &&
                !method.IsStatic &&
                !method.HasAttribute(NonActionAttribute);
        }

        public bool HasMethod(IMethodSymbol method)
        {
            return true;
        }
    }
}
