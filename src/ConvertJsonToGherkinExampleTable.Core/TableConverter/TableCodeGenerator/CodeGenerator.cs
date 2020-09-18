using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter.TableCodeGenerator
{
    internal static class CodeGenerator
    {
        private const string DefaultPaddingProperty = "    ";
        private const int FirstLetterIndex = 0;
        private static readonly Index LastIndex = ^1;

        public static string? FromTableConverterResult(TableConverterResult? tableConverterResult)
        {
            if (tableConverterResult is null)
                return default;

            var pairProperties = TransformToPairProperties(tableConverterResult);
            return TransformToStringPayload(pairProperties);
        }

        private static Dictionary<string, string>? TransformToPairProperties(TableConverterResult tableConverterResult)
        {
            if (string.IsNullOrEmpty(tableConverterResult.Headers) ||
                string.IsNullOrEmpty(tableConverterResult.Fields))
                return default;

            var headerList = tableConverterResult.Headers.Split(TableConvertionConstants.DefaultColumnSeparator);
            var resultList = new Dictionary<string, string>();

            foreach (var header in headerList)
            {
                if (string.IsNullOrEmpty(header))
                    continue;

                resultList.Add(NormalizeHeader(header), header);
            }

            return resultList;
        }

        private static string NormalizeHeader(string header) => header.Split(TableConvertionConstants.DefaultJoinSymbol)[LastIndex];

        private static string TransformToStringPayload(Dictionary<string, string>? pairProperties)
        {
            if (pairProperties is null || !pairProperties.Any())
                return string.Empty;

            var builder = new StringBuilder();
            builder.Append("using TechTalk.SpecFlow.Assist.Attributes;")
                   .Append(Environment.NewLine)
                   .Append(Environment.NewLine)
                   .Append("public class AutomaticTableGenerated")
                   .Append(Environment.NewLine)
                   .Append("{")
                   .Append(Environment.NewLine)
                   .Append(Environment.NewLine);

            foreach (var (propName, headerAttribute) in pairProperties)
            {
                builder.Append(DefaultPaddingProperty)
                       .Append("[TableAliasesAttribute(\"")
                       .Append(headerAttribute)
                       .Append("\")]")
                       .Append(Environment.NewLine)
                       .Append(DefaultPaddingProperty)
                       .Append("public string? ")
                       .Append(CapitalizeString(propName))
                       .Append(" ")
                       .Append("{get; set;}")
                       .Append(Environment.NewLine)
                       .Append(Environment.NewLine);
            }

            builder.Append(Environment.NewLine)
                   .Append("}");

            return builder.ToString();
        }

        private static string CapitalizeString(string text) => $"{char.ToUpper(text[FirstLetterIndex], CultureInfo.InvariantCulture)}{text[1..]}";
    }
}
