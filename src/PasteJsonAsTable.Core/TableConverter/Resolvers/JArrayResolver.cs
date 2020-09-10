using System.Linq;
using Newtonsoft.Json.Linq;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter.Resolvers
{
    public static class JArrayResolver
    {
        public static string ResolveValue(object value) => (value as JArray).Aggregate((previous, current) => $"{previous},{current}").ToString();
    }
}
