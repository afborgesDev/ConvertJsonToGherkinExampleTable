using System.Linq;
using Newtonsoft.Json.Linq;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter.Resolvers
{
    internal static class JArrayResolver
    {
        public static string ResolveValue(object value) => (value as JArray).Aggregate((previous, current) => $"{previous},{current}").ToString();
    }
}
