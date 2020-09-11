namespace ConvertJsonToGherkinExampleTable.Core
{
    /// <summary>
    /// Methods that enable to convert a bunch of json types into table <br/>
    /// all documenation here: <see href="https://github.com/afborgesDev/ConvertJsonToGherkinExampleTable"/>
    /// </summary>
    public interface IJsonConverterToExampleTable
    {
        /// <summary>
        /// Convert a payload into a table with single line
        /// </summary>
        /// <param name="jsonPayload">The string representation of the json payload</param>
        /// <returns>the string representation of the table with headers and fields</returns>
        string Convert(string jsonPayload);

        /// <summary>
        /// Convert multiple equals json into the same table with multiple lines
        /// </summary>
        /// <param name="jsonPayloads">the string representation of each json payload</param>
        /// <returns>A table with single line of headers and multiple lines of content</returns>
        string ConvertMultipleIntoSingleTable(params string[] jsonPayloads);
    }
}
