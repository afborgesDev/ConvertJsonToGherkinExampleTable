using System.Diagnostics.CodeAnalysis;

namespace ConvertJsonToGherkinExampleTable.CLI.Options
{
    [ExcludeFromCodeCoverage]
    public class DestinationFolderOption
    {
        public DestinationFolderOption(string? destFolder) => DestFolder = destFolder;

        public string? DestFolder { get; set; }
    }
}
