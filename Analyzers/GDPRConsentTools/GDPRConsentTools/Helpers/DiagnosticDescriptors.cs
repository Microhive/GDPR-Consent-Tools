// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Mvc.Analyzers
{
    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor GDPR1000_ClassMustBeMarkedSensitive =
            new DiagnosticDescriptor(
                "GDPR1000",
                "Class Declarataions containing properties with attribute [SensitiveAttribute] must have the attribute [SensitiveAttribute].",
                "Class Declarataions containing properties with attribute [SensitiveAttribute] must have the attribute [SensitiveAttribute].",
                "Usage",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor GDPR1001_SensitiveMustBeMarkedPurpose =
            new DiagnosticDescriptor(
                "GDPR1001",
                "Declarations containing entities with properties marked [SensitiveAttribute] must be marked [PurposeAttribute].",
                "Declarations containing entities with properties marked [SensitiveAttribute] must be marked [PurposeAttribute].",
                "Usage",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor GDPR1002_StorePurposeInPolicies =
            new DiagnosticDescriptor(
                "GDPR1002",
                "Method declarations with PurposeAttribute must be saved in Policies.cs",
                "Method declarations with PurposeAttribute must be saved in Policies.cs",
                "Usage",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor GDPR1003_PurposeMustContainName =
            new DiagnosticDescriptor(
                "GDPR1003",
                "PurposeAttribute must contain name.",
                "PurposeAttribute must contain name.",
                "Usage",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor GDPR1004_MethodAccessingPIIMustHavePurpose =
            new DiagnosticDescriptor(
                "GDPR1004",
                "Method using objects with type tagged with PIIAttribute must be tagged by PurposeAttribute.",
                "Method using objects with type tagged with PIIAttribute must be tagged by PurposeAttribute.",
                "Usage",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor GDPR1005_IdentityDbContextWithPurpose =
            new DiagnosticDescriptor(
                "GDPR1005",
                "IdentityDbContext used with PurposeAttribute must consume purpose.",
                "IdentityDbContext used with PurposeAttribute must consume purpose.",
                "Usage",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true);
    }
}
