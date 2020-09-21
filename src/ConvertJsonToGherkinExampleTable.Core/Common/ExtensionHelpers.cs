using System;
using ConvertJsonToGherkinExampleTable.Core.TableConverter;

namespace ConvertJsonToGherkinExampleTable.Core.Common
{
    internal static class ExtensionHelpers
    {
        private const string DoubleColumnSeparator = TableConvertionConstants.DefaultColumnSeparator + TableConvertionConstants.DefaultColumnSeparator;

        public static string RemoveSeparatorDuplicaitons(this string self) =>
            self.Replace(DoubleColumnSeparator, TableConvertionConstants.DefaultColumnSeparator, StringComparison.InvariantCultureIgnoreCase)
                .Replace(DoubleColumnSeparator, TableConvertionConstants.DefaultColumnSeparator, StringComparison.InvariantCultureIgnoreCase);
    }
}
