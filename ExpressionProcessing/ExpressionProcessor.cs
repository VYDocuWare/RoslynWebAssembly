using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;

namespace DocuWare.WorkflowDesigner.ExpressionProcessing
{
    public static partial class ExpressionProcessor
    {
        public static async Task<SemanticModel> ProcessExpression(MetadataReference visualBasicReference, MetadataReference systemDiagnosticsDebugReference, MetadataReference systemRuntimeReference)
        {
            var semanticModel = await GetProcessingResult(visualBasicReference, systemDiagnosticsDebugReference, systemRuntimeReference);
            return semanticModel;

        }

        public static async Task<SemanticModel> GetProcessingResult(MetadataReference visualBasicReference, MetadataReference systemDiagnosticsDebugReference, MetadataReference systemRuntimeReference)
        {

            var codeTemplate = $@"
                Option Strict On

                Imports System
                Imports Microsoft.VisualBasic
                Imports Microsoft.VisualBasic.CompilerServices
                Imports Microsoft.VisualBasic.Financial
                Module Module1
                    Sub TempWrapperFunction()
123={{0}}
                    End Sub

                    Sub Main()
                    End Sub
                End Module
                ";
            var fullCode = string.Format(codeTemplate, "345");

            var tree = VisualBasicSyntaxTree.ParseText(fullCode);


            var compilation = VisualBasicCompilation.Create("TemporaryCompilation",
                syntaxTrees: new[] { tree },
                references: new[]
                {
                    visualBasicReference,
                    systemDiagnosticsDebugReference,
                    systemRuntimeReference,
                });

            var semanticModel = compilation.GetSemanticModel(tree);
            return semanticModel;
        }
    }
}
