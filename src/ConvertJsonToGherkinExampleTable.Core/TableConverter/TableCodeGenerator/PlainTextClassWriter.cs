using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter.TableCodeGenerator
{
    internal static class PlainTextClassWriter
    {
        public static string FromDictionary(Dictionary<string, string>? pairProperties)
        {
            if (pairProperties is null || !pairProperties.Any())
                return string.Empty;

            var builder = new StringBuilder();
            AddClassHeader(ref builder);
            AddClassProperties(pairProperties, ref builder);
            AddClassFooter(builder);
            return builder.ToString();
        }

        private static void AddClassHeader(ref StringBuilder builder) =>
            builder.Append("using TechTalk.SpecFlow.Assist.Attributes;")
                   .Append(Environment.NewLine).Append(Environment.NewLine)
                   .Append("public class AutomaticTableGenerated")
                   .Append(Environment.NewLine)
                   .Append("{")
                   .Append(Environment.NewLine).Append(Environment.NewLine);

        private static void AddClassProperties(Dictionary<string, string>? pairProperties, ref StringBuilder builder)
        {
            if (pairProperties is null || !pairProperties.Any())
                return;

            foreach (var (propName, headerAttribute) in pairProperties)
            {
                builder.Append(CogeGenerationUtils.DefaultPaddingProperty)
                       .Append("[TableAliasesAttribute(\"")
                       .Append(headerAttribute)
                       .Append("\")]")
                       .Append(Environment.NewLine)
                       .Append(CogeGenerationUtils.DefaultPaddingProperty)
                       .Append("public string? ")
                       .Append(CogeGenerationUtils.CapitalizeString(propName))
                       .Append(" ")
                       .Append("{get; set;}")
                       .Append(Environment.NewLine)
                       .Append(Environment.NewLine);
            }
        }

        private static void AddClassFooter(StringBuilder builder) => builder.Append(Environment.NewLine).Append("}");
    }
}
