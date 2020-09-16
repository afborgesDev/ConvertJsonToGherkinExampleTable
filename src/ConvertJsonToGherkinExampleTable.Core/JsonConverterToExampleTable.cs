using System.Collections.Generic;
using System.Linq;
using ConvertJsonToGherkinExampleTable.Core.JsonParser;
using ConvertJsonToGherkinExampleTable.Core.TableConverter;

namespace ConvertJsonToGherkinExampleTable.Core
{
    /// <summary>
    /// Methods that enable to convert a bunch of json types into table <br/>
    /// all documenation here: <see href="https://github.com/afborgesDev/ConvertJsonToGherkinExampleTable"/>
    /// </summary>
    public sealed class JsonConverterToExampleTable : IJsonConverterToExampleTable
    {
        /// <summary>
        /// Default message when couldn't convert the json into a dictionary
        /// </summary>
        public const string CouldNotConvertJsonIntoTableMessage = "Could not convert the Json into a Table";

        /// <summary>
        /// Default message when one or more of json to convert into a single table are different.
        /// </summary>
        public const string AllJsonsShouldHaveSameFieldNamsToConvertMultipleIntoOneTable = "All JSON should have the same field names to convert into a single table";

        /// <summary>
        /// Convert a payload into a table with single line
        /// </summary>
        /// <param name="jsonPayload">The string representation of the json payload</param>
        /// <param name="generateCode">Indicate that if the convertion should generate a table C# implementation</param>
        /// <returns>the string representation of the table with headers and fields</returns>
        public string? Convert(string jsonPayload, bool generateCode = false)
        {
            if (!ParseToDictionaryHelper.TryParseToDictionary(jsonPayload, out var parsedJson))
                return CouldNotConvertJsonIntoTableMessage;

            return Converter.Convert(parsedJson, generateCode).ToString();
        }

        /// <summary>
        /// Methods that enable to convert a bunch of json types into table <br/>
        /// all documenation here: <see href="https://github.com/afborgesDev/ConvertJsonToGherkinExampleTable"/>
        /// </summary>
        public string? ConvertMultipleIntoSingleTable(bool generateCode = false, params string[] jsonPayloads)
        {
            var convertResultList = new List<TableConverterResult>();

            foreach (var payload in jsonPayloads)
            {
                if (!ParseToDictionaryHelper.TryParseToDictionary(payload, out var parsedJson))
                    return CouldNotConvertJsonIntoTableMessage;

                var convertResult = Converter.Convert(parsedJson);
                convertResultList.Add(convertResult);
            }

            return JoinResults(convertResultList, generateCode);
        }

        private static string? JoinResults(List<TableConverterResult> converterResults, bool generateCode)
        {
            var header = converterResults.Select(x => x.Headers).First();

            if (converterResults.Any(x => x.Headers != header))
                return AllJsonsShouldHaveSameFieldNamsToConvertMultipleIntoOneTable;

            var fields = converterResults.Select(x => x.Fields);
            var mainResult = TableConverterResult.FromHeaderAndListFields(header, fields, generateCode);
            return mainResult.ToString();
        }
    }
}
