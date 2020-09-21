using System;
using System.Globalization;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter.TableCodeGenerator
{
    internal static class CogeGenerationUtils
    {
        public const string DefaultPaddingProperty = "    ";
        public const int FirstLetterIndex = 0;
        public static readonly Index LastIndex = ^1;

        public static string CapitalizeString(string text) => $"{char.ToUpper(text[FirstLetterIndex], CultureInfo.InvariantCulture)}{text[1..]}";
    }
}
