using System;
using System.Linq;
using ConvertJsonToGherkinExampleTable.Core;
using ConvertJsonToGherkinExampleTable.Test.ParserTest;
using FluentAssertions;
using Xunit;

namespace ConvertJsonToGherkinExampleTable.Test.CodeGenerationTest
{
    public class CodeGenTest
    {
        private const int CountFieldsAndCode = 2;
        private static readonly string ExpectedCodeGenerated = $"using TechTalk.SpecFlow.Assist.Attributes;{Environment.NewLine}{Environment.NewLine}public class AutomaticTableGenerated{Environment.NewLine}{{{Environment.NewLine}{Environment.NewLine}    [TableAliasesAttribute(\"name\")]{Environment.NewLine}    public string? Name {{get; set;}}{Environment.NewLine}{Environment.NewLine}    [TableAliasesAttribute(\"Age\")]\r\n    public string? Age {{get; set;}}{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}}}";
        private static readonly string ExpectedNormalizedwithAttribute = $"using TechTalk.SpecFlow.Assist.Attributes;{Environment.NewLine}{Environment.NewLine}public class AutomaticTableGenerated{Environment.NewLine}{{{Environment.NewLine}{Environment.NewLine}    [TableAliasesAttribute(\"name\")]{Environment.NewLine}    public string? Name {{get; set;}}{Environment.NewLine}{Environment.NewLine}    [TableAliasesAttribute(\"Basket.IsEmpty\")]{Environment.NewLine}    public string? IsEmpty {{get; set;}}{Environment.NewLine}{Environment.NewLine}    [TableAliasesAttribute(\"Basket.IsFromRefound\")]{Environment.NewLine}    public string? IsFromRefound {{get; set;}}{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}}}";
        private static readonly string SeparatorIdentify = $"{Environment.NewLine}**CODE:**";

        [Fact]
        public void SimpleConvertionWithCodeGenOptionShouldCreateTableAndCodeOutput()
        {
            var sut = new JsonConverterToExampleTable();
            var convertionResult = sut.Convert(PayloadLoader.GetPayloadAsString("TwoItemsPayload"), true);
            AssertConvertionResult(convertionResult, JsonParserGeneralTest.ExpectedTableTwoItemsPayload, ExpectedCodeGenerated);
        }

        [Fact]
        public void ConvertionWithCodeGenOptionShouldReturnNormalizedAndWithTheRightAttribute()
        {
            var sut = new JsonConverterToExampleTable();
            var convertionResult = sut.Convert(PayloadLoader.GetPayloadAsString("SimplePayloadWithInsideObject"), true);
            AssertConvertionResult(convertionResult, JsonParserGeneralTest.SimplePayloadWithInsideObjectExpectedTable, ExpectedNormalizedwithAttribute);
        }

        private static void AssertConvertionResult(string convertionResult, string expectedTable, string expectedCode)
        {
            convertionResult.Should().NotBeNullOrEmpty();
            var items = convertionResult.Split(SeparatorIdentify);
            items.Count().Should().Be(CountFieldsAndCode);

            var foundTable = items.Any(x => x.Equals(expectedTable, StringComparison.InvariantCultureIgnoreCase));
            var foundCode = items.Any(x => x.Contains(expectedCode, StringComparison.InvariantCultureIgnoreCase));

            foundTable.Should().BeTrue();
            foundCode.Should().BeTrue();
        }

        //Test with . separator
        //Test multiple
    }
}
