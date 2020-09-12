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
        /// Convert a payload into a table with single line
        /// </summary>
        /// <param name="jsonPayload">The string representation of the json payload</param>
        /// <returns>the string representation of the table with headers and fields</returns>
        public string? Convert(string jsonPayload)
        {
            if (!ParseToDictionaryHelper.TryParseToDictionary(jsonPayload, out var parsedJson))
                return CouldNotConvertJsonIntoTableMessage;

            return Converter.Convert(parsedJson).ToString();
        }

        /// <summary>
        /// Methods that enable to convert a bunch of json types into table <br/>
        /// all documenation here: <see href="https://github.com/afborgesDev/ConvertJsonToGherkinExampleTable"/>
        /// </summary>
        public string? ConvertMultipleIntoSingleTable(params string[] jsonPayloads)
        {
            var convertResultList = new List<TableConverterResult>();

            foreach (var payload in jsonPayloads)
            {
                if (!ParseToDictionaryHelper.TryParseToDictionary(payload, out var parsedJson))
                    return CouldNotConvertJsonIntoTableMessage;

                convertResultList.Add(Converter.Convert(parsedJson));
            }

            return JoinResults(convertResultList);
        }

        private static string? JoinResults(List<TableConverterResult> converterResults)
        {
            var header = converterResults.Select(x => x.Headers).First();
            var fields = converterResults.Select(x => x.Fields);
            var mainResult = TableConverterResult.FromHeaderAndListFields(header, fields);
            return mainResult.ToString();
        }
    }
}
