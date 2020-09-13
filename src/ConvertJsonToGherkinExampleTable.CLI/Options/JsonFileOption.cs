namespace ConvertJsonToGherkinExampleTable.CLI.Options
{
    public class JsonFileOption
    {
        public JsonFileOption(string? jsonFile) => JsonFile = jsonFile;

        public string? JsonFile { get; set; }
    }
}
