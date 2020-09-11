using System;
using System.Collections.Generic;
using ConvertJsonToGherkinExampleTable.Core.TableConverter.Resolvers;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter
{
    internal static class Converter
    {
        public static string Convert(Dictionary<string, object> json) =>
            BuildResult(HeaderResolver.Resolve(json), FieldResolver.Resolve(json));

        private static string BuildResult(string headers, string fields) =>
            CleanCovertedText($"{TableConvertionConstants.DefaultColumnSeparator}{headers}{Environment.NewLine}{TableConvertionConstants.DefaultColumnSeparator}{fields}");

        private static string CleanCovertedText(string text)
        {
            var temp = text.Replace($"{TableConvertionConstants.DefaultColumnSeparator}{TableConvertionConstants.DefaultColumnSeparator}",
                                       TableConvertionConstants.DefaultColumnSeparator, StringComparison.InvariantCultureIgnoreCase);

            return temp.Replace($"{TableConvertionConstants.DefaultColumnSeparator}{TableConvertionConstants.DefaultColumnSeparator}",
                                   TableConvertionConstants.DefaultColumnSeparator, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
