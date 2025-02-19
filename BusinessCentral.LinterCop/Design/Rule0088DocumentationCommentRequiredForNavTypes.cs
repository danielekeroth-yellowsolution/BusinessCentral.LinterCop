using BusinessCentral.LinterCop.Helpers;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace BusinessCentral.LinterCop.Design;

[DiagnosticAnalyzer]
public class Rule0088InternalProcedureModifier : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(DiagnosticDescriptors.Rule0088DocumentationCommentRequiredForNavTypes);

    public override void Initialize(AnalysisContext context)
        => context.RegisterSymbolAction(new Action<SymbolAnalysisContext>(this.CheckForMissingCaptions),
            SymbolKind.Page,
            SymbolKind.Query,
            SymbolKind.Table,
            SymbolKind.Field,
            SymbolKind.Action,
            SymbolKind.EnumValue,
            SymbolKind.Control,
            SymbolKind.Codeunit
        );

    private void CheckForMissingCaptions(SymbolAnalysisContext context)
    {   
        if (context.IsObsoletePendingOrRemoved())
            return;

        if(string.IsNullOrWhiteSpace(context.Symbol.GetDocumentationCommentXml()))
        {
                context.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.Rule0088DocumentationCommentRequiredForNavTypes,
                context.Symbol.GetLocation()));
        }
    }
}