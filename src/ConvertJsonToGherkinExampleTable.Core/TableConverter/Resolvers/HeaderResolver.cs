using System.Collections.Generic;
using System.Text;
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
                var valueType = value.GetType();

                if (!valueType.IsJObject())
                {
                    builder.Append(key).Append(TableConvertionConstants.DefaultColumnSeparator);
                    continue;
                }

                _ = ParseToDictionaryHelper.TryParseToDictionary(value.ToString(), out var insideJson);
                var objectReduceResult = Resolve(insideJson).Split(TableConvertionConstants.DefaultColumnSeparator);
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

            return builder.ToString();
        }
    }
}
