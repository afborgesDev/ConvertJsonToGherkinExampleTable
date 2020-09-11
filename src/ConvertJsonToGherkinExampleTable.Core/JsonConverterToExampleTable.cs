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
        public string Convert(string jsonPayload)
        {
            if (!ParseToDictionaryHelper.TryParseToDictionary(jsonPayload, out var parsedJson))
                return CouldNotConvertJsonIntoTableMessage;

            return Converter.Convert(parsedJson);
        }

        /// <summary>
        /// Methods that enable to convert a bunch of json types into table <br/>
        /// all documenation here: <see href="https://github.com/afborgesDev/ConvertJsonToGherkinExampleTable"/>
        /// </summary>
        public string ConvertMultipleIntoSingleTable(params string[] jsonPayloads)
        {
            throw new System.NotImplementedException();
        }
    }
}
