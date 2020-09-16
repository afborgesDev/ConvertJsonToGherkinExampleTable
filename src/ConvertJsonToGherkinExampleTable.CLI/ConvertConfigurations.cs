using System.Diagnostics.CodeAnalysis;

namespace ConvertJsonToGherkinExampleTable.CLI
{
    [ExcludeFromCodeCoverage]
    public class ConvertConfigurations
    {
        public string? FilePath { get; set; }
        public string? FolderPath { get; set; }
        public string? ResultFilePath { get; set; }
        public bool? PayloadFromClipboard { get; set; }
        public bool? GenerateCode { get; set; }
        public string? DestinationGeneratedCode { get; set; }
    }
}
