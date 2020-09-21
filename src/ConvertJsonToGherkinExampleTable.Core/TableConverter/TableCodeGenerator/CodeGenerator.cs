using System.Collections.Generic;
using System.Linq;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter.TableCodeGenerator
{
    internal static class CodeGenerator
    {
        public static string? FromTableConverterResult(TableConverterResult? tableConverterResult)
        {
            if (tableConverterResult is null)
                return default;

            return PlainTextClassWriter.FromDictionary(TransformToPairProperties(tableConverterResult));
        }

        private static Dictionary<string, string>? TransformToPairProperties(TableConverterResult tableConverterResult)
        {
            if (string.IsNullOrEmpty(tableConverterResult.Headers) ||
                string.IsNullOrEmpty(tableConverterResult.Fields))
                return default;

            var headerList = TransformHeaderTextToList(tableConverterResult.Headers);
            return headerList.Where(x => !string.IsNullOrEmpty(x))
                             .ToDictionary(key => NormalizeHeader(key), header => header);
        }

        private static string[]? TransformHeaderTextToList(string? header) => header?.Split(TableConvertionConstants.DefaultColumnSeparator);

        private static string NormalizeHeader(string header) => header.Split(TableConvertionConstants.DefaultJoinSymbol)[CogeGenerationUtils.LastIndex];
    }
}
