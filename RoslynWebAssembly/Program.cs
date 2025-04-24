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
            try
            {
                var expressionOutcome = ExpressionProcessor.ProcessExpression();
                return expressionOutcome.IsSpeculativeSemanticModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return false;
            }
        }
    }
}
