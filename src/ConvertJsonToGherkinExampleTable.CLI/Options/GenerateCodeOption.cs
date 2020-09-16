using System.Diagnostics.CodeAnalysis;

namespace ConvertJsonToGherkinExampleTable.CLI.Options
{
    [ExcludeFromCodeCoverage]
    public class GenerateCodeOption
    {
        public GenerateCodeOption(bool? genCode) => GenCode = genCode;

        public bool? GenCode { get; set; }
    }
}
