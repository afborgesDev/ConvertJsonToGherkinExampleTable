using System;
using System.Collections.Generic;
using System.Text;
using ConvertJsonToGherkinExampleTable.Core.JsonParser;
using ConvertJsonToGherkinExampleTable.Core.TableConverter.Resolvers;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter
{
    public static class Converter
    {
        public const string DefaultJoinSymbol = ".";
        public const string DefaultColumnSeparator = "|";

        public static string Convert(Dictionary<string, object> json)
        {
            var headers = BuildHeaders(json);
            var fields = BuildFields(json);

            return CleanCovertedText($"{DefaultColumnSeparator}{headers}{Environment.NewLine}{DefaultColumnSeparator}{fields}");
        }

        private static string BuildHeaders(Dictionary<string, object> json)
        {
            var builder = new StringBuilder();
            foreach (var (key, value) in json)
            {
                var valueType = value.GetType();

                if (!valueType.IsJObject())
                {
                    builder.Append(key).Append(DefaultColumnSeparator);
                    continue;
                }

                _ = Parser.TryParseIntoDynamicJson(value.ToString(), out var insideJson);
                var objectReduceResult = BuildHeaders(insideJson).Split(DefaultColumnSeparator);
                foreach (var insideItem in objectReduceResult)
                {
                    if (string.IsNullOrEmpty(insideItem))
                        continue;

                    builder.Append(key)
                           .Append(DefaultJoinSymbol)
                           .Append(insideItem)
                           .Append(DefaultColumnSeparator);
                }
            }

            return builder.ToString();
        }

        private static string BuildFields(Dictionary<string, object> json)
        {
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
                    if (Parser.TryParseIntoDynamicJson(value.ToString(), out var insideJson))
                    {
                        builder.Append(BuildFields(insideJson))
                               .Append(DefaultColumnSeparator);
                        continue;
                    }

                builder.Append(value.ToString())
                       .Append(DefaultColumnSeparator);
            }

            return builder.ToString();
        }

        private static string CleanCovertedText(string text)
        {
            var temp = text.Replace($"{DefaultColumnSeparator}{DefaultColumnSeparator}", DefaultColumnSeparator, StringComparison.InvariantCultureIgnoreCase);
            return temp.Replace($"{DefaultColumnSeparator}{DefaultColumnSeparator}", DefaultColumnSeparator, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
