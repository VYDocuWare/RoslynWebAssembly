using DocuWare.WorkflowDesigner.ExpressionProcessing;
using System.Runtime.InteropServices.JavaScript;

namespace RoslynWebAssembly
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(GetCompiledResult());
        }

        [JSExport]
        static bool GetCompiledResult()
        {

            var expressionOutcome = ExpressionProcessor.ProcessExpression();
            return expressionOutcome.IsSpeculativeSemanticModel;
        }
    }
}
