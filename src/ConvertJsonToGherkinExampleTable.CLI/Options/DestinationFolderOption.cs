namespace ConvertJsonToGherkinExampleTable.CLI.Options
{
    public class DestinationFolderOption
    {
        public DestinationFolderOption(string? destFolder) => DestFolder = destFolder;

        public string? DestFolder { get; set; }
    }
}
