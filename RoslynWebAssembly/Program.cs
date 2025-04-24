using DocuWare.WorkflowDesigner.ExpressionProcessing;
using Microsoft.CodeAnalysis;
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
        async static Task<bool> GetCompiledResult()
        {
            try
            {

                //var visualBasicAssemblyLocation = "C:/Program Files/dotnet/shared/Microsoft.NETCore.App/9.0.4/Microsoft.VisualBasic.Core.dll";

                //var systemDiagnosticsDebugAssemblyLocation = "C:/Program Files/dotnet/shared/Microsoft.NETCore.App/9.0.4/System.Private.CoreLib.dll";

                //var systemRuntimeAssemblyLocation = "C:/Program Files/dotnet/shared/Microsoft.NETCore.App/9.0.4/System.Runtime.dll";

                var visualBasicAssemblyLocation = "http://localhost:8000/Microsoft.VisualBasic.Core.dll";

                var systemDiagnosticsDebugAssemblyLocation = "http://localhost:8000/System.Private.CoreLib.dll";

                var systemRuntimeAssemblyLocation = "http://localhost:8000/System.Runtime.dll";


                var visualBasicReferenceRequest = ResolveReferenceAsync(visualBasicAssemblyLocation);
                var systemDiagnosticsDebugReferenceRequest = ResolveReferenceAsync(systemDiagnosticsDebugAssemblyLocation);
                var systemRuntimeReferenceRequest = ResolveReferenceAsync(systemRuntimeAssemblyLocation);

                await Task.WhenAll(
                    visualBasicReferenceRequest,
                    systemDiagnosticsDebugReferenceRequest,
                    systemRuntimeReferenceRequest
                );


                var visualBasicReference = await visualBasicReferenceRequest;
                var systemDiagnosticsDebugReference = await systemDiagnosticsDebugReferenceRequest;
                var systemRuntimeReference = await systemRuntimeReferenceRequest;


                var expressionOutcome = await ExpressionProcessor.ProcessExpression(visualBasicReference, systemDiagnosticsDebugReference, systemRuntimeReference);

                return expressionOutcome.IsSpeculativeSemanticModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return false;
            }
        }

        public static async Task<MetadataReference> ResolveReferenceAsync(string url)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var byteStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return MetadataReference.CreateFromStream(byteStream, new(MetadataImageKind.Assembly));
        }
    }
}
