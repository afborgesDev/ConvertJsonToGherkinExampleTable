using System.Diagnostics.CodeAnalysis;

namespace ConvertJsonToGherkinExampleTable.CLI.Options
{
    [ExcludeFromCodeCoverage]
    public class JsonFolderOption
    {
        public JsonFolderOption(string? jsonFolder) => JsonFolder = jsonFolder;

        public string? JsonFolder { get; set; }
    }
}
