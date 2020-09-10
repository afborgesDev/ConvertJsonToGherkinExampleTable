using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace ConvertJsonToGherkinExampleTable.Test
{
    public static class PayloadLoader
    {
        private const string AssemblyName = "ConvertJsonToGherkinExampleTable.Test";

        public static string GetPayloadAsString(string payloadFile) => ReadFileResource($"payloads.{payloadFile}.json");

        public static T GetPayload<T>(string payloadName)
        {
            var payloadAsString = GetPayloadAsString(payloadName);
            return JsonConvert.DeserializeObject<T>(payloadAsString);
        }

        private static string ReadFileResource(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var resourceStream = assembly.GetManifestResourceStream($"{AssemblyName}.{resourcePath}");
            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}
