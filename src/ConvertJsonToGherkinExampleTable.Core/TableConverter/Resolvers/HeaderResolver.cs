using System.Collections.Generic;
using System.Text;
using ConvertJsonToGherkinExampleTable.Core.Common;
using ConvertJsonToGherkinExampleTable.Core.JsonParser;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter.Resolvers
{
    internal static class HeaderResolver
    {
        public static string Resolve(Dictionary<string, object> json)
        {
            var builder = new StringBuilder();
            foreach (var (key, value) in json)
            {
                if (!value.GetType().IsJObject())
                {
                    builder.Append(key).Append(TableConvertionConstants.DefaultColumnSeparator);
                    continue;
                }

                _ = ParseToDictionaryHelper.TryParseToDictionary(value.ToString(), out var insideJson);
                var objectReduceResult = Resolve(insideJson).Split(TableConvertionConstants.DefaultColumnSeparator);
                ReduceInsideItem(key, objectReduceResult, ref builder);
            }

            return builder.ToString();
        }

        private static void ReduceInsideItem(string key, string[]? objectReduceResult, ref StringBuilder builder)
        {
            if (objectReduceResult is null)
                return;

            foreach (var insideItem in objectReduceResult)
            {
                if (string.IsNullOrEmpty(insideItem))
                    continue;

                builder.Append(key)
                       .Append(TableConvertionConstants.DefaultJoinSymbol)
                       .Append(insideItem)
                       .Append(TableConvertionConstants.DefaultColumnSeparator);
            }
        }
    }
}
