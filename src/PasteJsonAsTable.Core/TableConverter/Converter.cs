using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using PasteJsonAsTable.Core.JsonParser;
using PasteJsonAsTable.Core.TableConverter.Resolvers;

namespace PasteJsonAsTable.Core.TableConverter
{
    public static class Converter
    {
        public const string DefaultColumnSeparator = "|";

        public static string Convert(Dictionary<string, object> json)
        {
            var headers = CleanCovertedText(BuildHeaders(json));
            var fields = CleanCovertedText(BuildFields(json));

            return $"{DefaultColumnSeparator}{headers}{Environment.NewLine}{DefaultColumnSeparator}{fields}";
        }

        private static Func<string, string, string> AggregationToReduce() => (previous, current) => $"{previous}{DefaultColumnSeparator}{current}{DefaultColumnSeparator}";

        private static string BuildHeaders(Dictionary<string, object> json) => json.Select(x => ConvertJsonFieldToHeader(x)).Aggregate(AggregationToReduce()).ToString(CultureInfo.InvariantCulture);

        private static string BuildFields(Dictionary<string, object> json)
        {
            //check the field
            //If it is a object need to reduce the object
            //if it is a array need to flat the array
            //Transform it in a upper class that can see anyone.

            var builder = new StringBuilder();
            foreach (var (_, value) in json)
            {
                var valueType = value.GetType();
                if (valueType.IsJArray())
                {
                    builder.Append(JArrayResolver.ResolveValue(value))
                           .Append(DefaultColumnSeparator);
                    continue;
                }

                if (valueType.IsJObject())
                {
                    if (Parser.TryParseIntoDynamicJson(value.ToString(), out var insideJson))
                    {
                        builder.Append(BuildFields(insideJson))
                               .Append(DefaultColumnSeparator);
                        continue;
                    }
                }

                builder.Append(value.ToString())
                       .Append(DefaultColumnSeparator);
            }

            return builder.ToString();
            //return  json.Select(x => ConvertJsonValueToString(x)).Aggregate(AggregationToReduce()).ToString(CultureInfo.InvariantCulture);
        }

        private static string ConvertJsonFieldToHeader(KeyValuePair<string, object> jsonField)
        {
            var valueType = jsonField.Value.GetType();
            if (!valueType.IsJObject())
                return jsonField.Key;
            return JObjectResolver.ResolveHeaders(jsonField.Key, jsonField.Value);
        }

        private static string ConvertJsonValueToString(KeyValuePair<string, object> jsonField)
        {
            var valueType = jsonField.Value.GetType();

            if (valueType.IsJArray())
                return JArrayResolver.ResolveValue(jsonField.Value);

            if (valueType.IsJObject())
                return JObjectResolver.ResolveValues(jsonField.Value);

            return jsonField.Value.ToString();
        }

        private static string CleanCovertedText(string text) => text.Replace($"{DefaultColumnSeparator}{DefaultColumnSeparator}", DefaultColumnSeparator, StringComparison.InvariantCultureIgnoreCase);
    }
}
