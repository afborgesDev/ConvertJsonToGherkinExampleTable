using System.Collections.Generic;
using System.Text;
using ConvertJsonToGherkinExampleTable.Core.Common;
using ConvertJsonToGherkinExampleTable.Core.JsonParser;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter.Resolvers
{
    internal static class FieldResolver
    {
        public static string Resolve(Dictionary<string, object> json)
        {
            var builder = new StringBuilder();
            foreach (var (_, value) in json)
            {
                var valueType = value.GetType();
                if (valueType.IsJArray())
                {
                    builder.Append(JArrayResolver.ResolveValue(value))
                           .Append(TableConvertionConstants.DefaultColumnSeparator);
                    continue;
                }

                if (valueType.IsJObject() && ParseToDictionaryHelper.TryParseToDictionary(value.ToString(), out var insideJson))
                {
                    builder.Append(Resolve(insideJson))
                           .Append(TableConvertionConstants.DefaultColumnSeparator);
                    continue;
                }

                builder.Append(value.ToString())
                       .Append(TableConvertionConstants.DefaultColumnSeparator);
            }

            return builder.ToString();
        }
    }
}
