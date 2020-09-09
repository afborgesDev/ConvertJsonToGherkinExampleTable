using System;
using FluentAssertions;
using PasteJsonAsTable.Core.JsonParser;
using Xunit;

namespace PasteJsonAsTable.Test.ParserTest
{
    public class JsonParserGeneralTest
    {
        [Fact]
        public void ParseEmptyShouldReturnEmptyResult()
        {
            var sut = Parser.Parse(string.Empty);
            sut.Should().BeNullOrEmpty();
        }

        [Fact]
        public void ParseNullShouldReturnEmptyResult()
        {
            var sut = Parser.Parse(default);
            sut.Should().BeNullOrEmpty();
        }

        [Fact]
        public void PraseJsonWithNoColumnsShouldReturnEmptyResult()
        {
            var sut = Parser.Parse("{}");
            sut.Should().Be(Parser.CouldNotConvertJsonIntoTableMessage);
        }

        [Fact]
        public void TwhoFieldsJsonShouldParseToTwoColumnsTable()
        {
            var ExpectedTableResult = $"|name|Age|{Environment.NewLine}|this is a test|33|";

            var sut = Parser.Parse("{\"name\":\"this is a test\", \"Age\":33}");
            AssertValidTable(ExpectedTableResult, sut);
        }

        [Fact]
        public void ParseJsonWithArrayInsideShouldReturnValidTableWithListForTheArrayCollumn()
        {
            const string payload = "{\"name\":\"this is a test\", \"DaysPerWeekWorkOut\":[\"Sunday\", \"Wendsday\", \"Friday\"]}";
            var expectedTable = $"|name|DaysPerWeekWorkOut|{Environment.NewLine}|this is a test|Sunday,Wendsday,Friday|";

            var sut = Parser.Parse(payload);
            AssertValidTable(expectedTable, sut);
        }

        [Fact]
        public void ParseJsonWithInsideSimpleObjectSouldReturnAllItemsFlattedToValidTable()
        {
            const string payload = "{\"Name\": \"This is a test\", \"Basket\": { \"IsEmpty\": true, \"IsFromRefound\": false }}";
            var expectedTable = $"|Name|Basket.IsEmpty|Basket.IsFromRefound|{Environment.NewLine}|This is a test|True|False|";

            var sut = Parser.Parse(payload);
            AssertValidTable(expectedTable, sut);
        }

        private static void AssertValidTable(string ExpectedTableResult, string sut)
        {
            sut.Should().NotBeNullOrEmpty();
            sut.Should().BeEquivalentTo(ExpectedTableResult);
        }
    }
}
