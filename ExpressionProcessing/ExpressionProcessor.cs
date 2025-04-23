using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using System.Reflection;
using System.Xml.Linq;

namespace DocuWare.WorkflowDesigner.ExpressionProcessing
{
    public static partial class ExpressionProcessor
    {
        public static SemanticModel ProcessExpression()
        {
            var semanticModel = GetProcessingResult();
            return semanticModel;

        }

        public static SemanticModel GetProcessingResult()
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
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Microsoft.VisualBasic.Constants).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(XElement).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(System.Diagnostics.Debug).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Runtime, Version=6.0.0.0").Location),
                });

            var semanticModel = compilation.GetSemanticModel(tree);
            return semanticModel;
        }
    }
}
