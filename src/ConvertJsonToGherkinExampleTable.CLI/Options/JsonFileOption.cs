using System.Diagnostics.CodeAnalysis;

namespace ConvertJsonToGherkinExampleTable.CLI.Options
{
    [ExcludeFromCodeCoverage]
    public class JsonFileOption
    {
        public JsonFileOption(string? jsonFile) => JsonFile = jsonFile;

        public string? JsonFile { get; set; }
    }
}
