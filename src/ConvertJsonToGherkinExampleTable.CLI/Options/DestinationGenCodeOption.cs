using System.Diagnostics.CodeAnalysis;

namespace ConvertJsonToGherkinExampleTable.CLI.Options
{
    [ExcludeFromCodeCoverage]
    public class DestinationGenCodeOption
    {
        public DestinationGenCodeOption(string? destGenCode) => DestGenCode = destGenCode;

        public string? DestGenCode { get; set; }
    }
}
