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

        private static readonly string SeparatorIdentify = $"{Environment.NewLine}**CODE:**";

        private static readonly string[] ExpectedSimpleCode = new string[] {"using TechTalk.SpecFlow.Assist.Attributes;",
                                                                            "public class AutomaticTableGenerated",
                                                                            "[TableAliasesAttribute(\"name\")]",
                                                                            "public string? Name {get; set;}",
                                                                            "[TableAliasesAttribute(\"Age\")]",
                                                                            "public string? Age {get; set;}"};

        private static readonly string[] ExpectedNormalizedwithAttribute = new string[] {"using TechTalk.SpecFlow.Assist.Attributes",
                                                                                         "public class AutomaticTableGenerated",
                                                                                         "[TableAliasesAttribute(\"name\")]",
                                                                                         "public string? Name {get; set;}",
                                                                                         "[TableAliasesAttribute(\"Basket.IsEmpty\")]",
                                                                                         "public string? IsEmpty {get; set;}",
                                                                                         "[TableAliasesAttribute(\"Basket.IsFromRefound\")]",
                                                                                         "public string? IsFromRefound {get; set;}"};

        [Fact]
        public void SimpleConvertionWithCodeGenOptionShouldCreateTableAndCodeOutput()
        {
            var sut = new JsonConverterToExampleTable();
            var convertionResult = sut.Convert(PayloadLoader.GetPayloadAsString("TwoItemsPayload"), true);
            AssertConvertionResult(convertionResult, JsonParserGeneralTest.ExpectedTableTwoItemsPayload, ExpectedSimpleCode);
        }

        [Fact]
        public void ConvertionWithCodeGenOptionShouldReturnNormalizedAndWithTheRightAttribute()
        {
            var sut = new JsonConverterToExampleTable();
            var convertionResult = sut.Convert(PayloadLoader.GetPayloadAsString("SimplePayloadWithInsideObject"), true);
            AssertConvertionResult(convertionResult, JsonParserGeneralTest.SimplePayloadWithInsideObjectExpectedTable, ExpectedNormalizedwithAttribute);
        }

        private static void AssertConvertionResult(string convertionResult, string expectedTable, string[] expectedCode)
        {
            convertionResult.Should().NotBeNullOrEmpty();
            var items = convertionResult.Split(SeparatorIdentify);
            items.Count().Should().Be(CountFieldsAndCode);

            var foundTable = items.Any(x => x.Equals(expectedTable, StringComparison.InvariantCultureIgnoreCase));
            var foundCode = items[1];

            foreach (var item in expectedCode)
                foundCode.Contains(item, StringComparison.InvariantCultureIgnoreCase).Should().BeTrue($"expected to have: {item}");

            foundTable.Should().BeTrue(convertionResult);
        }

        //Test multiple
    }
}
