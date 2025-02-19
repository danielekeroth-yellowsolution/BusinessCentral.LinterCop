using BusinessCentral.LinterCop.Helpers;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace BusinessCentral.LinterCop.Design;

[DiagnosticAnalyzer]
public class Rule0089InternalProcedureModifier : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(DiagnosticDescriptors.Rule0088DocumentationCommentRequiredForNavTypes);

    public override void Initialize(AnalysisContext context) =>
        context.RegisterCodeBlockAction(new Action<CodeBlockAnalysisContext>(this.AnalyzeIdentifiersInEventSubscribers));

    private void AnalyzeIdentifiersInEventSubscribers(CodeBlockAnalysisContext ctx)
    {
        if (ctx.IsObsoletePendingOrRemoved() || !ctx.CodeBlock.IsKind(SyntaxKind.MethodDeclaration))
            return;

        if (ctx.CodeBlock is not MethodDeclarationSyntax syntax)
            return;

        if(!syntax.Attributes.Any(value => SemanticFacts.IsSameName(value.GetIdentifierOrLiteralValue() ?? "", "EventSubscriber")))
            return;

        if((ctx.CodeBlock as MethodDeclarationSyntax)?.Body.Statements.Count > 5)
            ctx.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.Rule0089EventSubscriberTooLarge,
                ctx.CodeBlock.GetLocation()));  
    }
}