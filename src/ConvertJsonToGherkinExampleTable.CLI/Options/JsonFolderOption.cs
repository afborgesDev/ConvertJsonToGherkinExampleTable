namespace ConvertJsonToGherkinExampleTable.CLI.Options
{
    public class JsonFolderOption
    {
        public JsonFolderOption(string? jsonFolder) => JsonFolder = jsonFolder;

        public string? JsonFolder { get; set; }
    }
}
