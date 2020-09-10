using System;
using ConvertJsonToGherkinExampleTable.Core.JsonParser;
using FluentAssertions;
using Xunit;

namespace ConvertJsonToGherkinExampleTable.Test.ParserTest
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
            var sut = Parser.Parse(PayloadLoader.GetPayloadAsString("TwoItemsPayload"));
            AssertValidTable(ExpectedTableResult, sut);
        }

        [Fact]
        public void ParseJsonWithArrayInsideShouldReturnValidTableWithListForTheArrayCollumn()
        {
            var expectedTable = $"|name|DaysPerWeekWorkOut|{Environment.NewLine}|this is a test|Sunday,Wendsday,Friday|";
            var sut = Parser.Parse(PayloadLoader.GetPayloadAsString("SimplePayloadWithArrayProperty"));
            AssertValidTable(expectedTable, sut);
        }

        [Fact]
        public void ParseJsonWithInsideSimpleObjectSouldReturnAllItemsFlattedToValidTable()
        {
            var expectedTable = $"|Name|Basket.IsEmpty|Basket.IsFromRefound|{Environment.NewLine}|This is a test|True|False|";
            var sut = Parser.Parse(PayloadLoader.GetPayloadAsString("SimplePayloadWithInsideObject"));
            AssertValidTable(expectedTable, sut);
        }

        [Fact]
        public void ParseJsonWithObjectAndArrayShouldReturnAllItemsFlattedToValidTable()
        {
            var expectedTable = $"|name|References.IsActive|References.Load|DaysToLoad|{Environment.NewLine}|This is a name|True|90|Monday,Whenesday,Friday,Saturday|";
            var sut = Parser.Parse(PayloadLoader.GetPayloadAsString("SimplePayloadWithObjectAndArray"));
            AssertValidTable(expectedTable, sut);
        }

        [Fact]
        public void ParseComplexJsonWithArrayInsideObjectSouldReturnAllItemsFlattedToValidTable()
        {
            var expectedTable = $"|Name|Configurations.IsActive|Configurations.ScheduledDays|LastUpdate|{Environment.NewLine}"
                              + $"|ComplexPayload|True|Monday,Saturday,Sunday|10/10/2022|";
            var sut = Parser.Parse(PayloadLoader.GetPayloadAsString("ComplexPayloadArrayInsideObject"));
            AssertValidTable(expectedTable, sut);
        }

        [Fact]
        public void ParseComplexJsonWithMultipleInsideObjectAndArrayShouldReturnAllItemsFlattedToValidTable()
        {
            var expectedTable = $"|Name|Configurations.IsActive|Configurations.ScheduledDays|LastUpdate"
                              + $"|UpdateInfo.Owner.FirstName|UpdateInfo.Owner.LastName|UpdateInfo.Owner.IsAdmin"
                              + $"|UpdateInfo.PipeUpdated.Count|UpdateInfo.PipeUpdated.HasSomeError"
                              + $"|UpdateInfo.PipeUpdated.ExecutionDetail.Log|{Environment.NewLine}"
                              + "|ComplexPayload|True|Monday,Saturday,Sunday|10/10/2022|User|Beta|False"
                              + "|2|False|Succeed,Succeed|";

            ExecuteAndAssert(PayloadLoader.GetPayloadAsString("ComplexPayloadMultipleInsideArrayAndObject"), expectedTable);
        }

        private static void ExecuteAndAssert(string payload, string expectedTable)
        {
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
