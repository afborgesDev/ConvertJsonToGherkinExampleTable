using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PasteJsonAsTable.Core.TableConverter.Resolvers;

namespace PasteJsonAsTable.Core.TableConverter
{
    public static class Converter
    {
        public const string DefaultColumnSeparator = "|";

        public static string Convert(Dictionary<string, object> json)
        {
            var headers = BuildHeaders(json);
            var fields = BuildFields(json);

            return $"{DefaultColumnSeparator}{headers}{Environment.NewLine}{DefaultColumnSeparator}{fields}";
        }

        private static Func<string, string, string> AggregationToReduce() => (previous, current) => $"{previous}{DefaultColumnSeparator}{current}{DefaultColumnSeparator}";

        private static string BuildHeaders(Dictionary<string, object> json) => json.Select(x => ConvertJsonFieldToHeader(x)).Aggregate(AggregationToReduce()).ToString(CultureInfo.InvariantCulture);

        private static string BuildFields(Dictionary<string, object> json) => json.Select(x => ConvertJsonValueToString(x)).Aggregate(AggregationToReduce()).ToString(CultureInfo.InvariantCulture);

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
    }
}
