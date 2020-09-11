using System;
using ConvertJsonToGherkinExampleTable.Core;
using FluentAssertions;
using Xunit;

namespace ConvertJsonToGherkinExampleTable.Test.ParserTest
{
    public class JsonParserGeneralTest
    {
        [Fact]
        public void ParseEmptyShouldReturnEmptyResult()
        {
            var sut = GetNewConverter().Convert(string.Empty);
            sut.Should().Be(JsonConverterToExampleTable.CouldNotConvertJsonIntoTableMessage);
        }

        [Fact]
        public void ParseNullShouldReturnEmptyResult()
        {
            var sut = GetNewConverter().Convert(default);
            sut.Should().Be(JsonConverterToExampleTable.CouldNotConvertJsonIntoTableMessage);
        }

        [Fact]
        public void PraseJsonWithNoColumnsShouldReturnEmptyResult()
        {
            var sut = GetNewConverter().Convert("{}");
            sut.Should().Be(JsonConverterToExampleTable.CouldNotConvertJsonIntoTableMessage);
        }

        [Fact]
        public void TwhoFieldsJsonShouldParseToTwoColumnsTable()
        {
            var expectedTable = $"|name|Age|{Environment.NewLine}|this is a test|33|";
            ExecuteAndAssert("TwoItemsPayload", expectedTable);
        }

        [Fact]
        public void ParseJsonWithArrayInsideShouldReturnValidTableWithListForTheArrayCollumn()
        {
            var expectedTable = $"|name|DaysPerWeekWorkOut|{Environment.NewLine}|this is a test|Sunday,Wendsday,Friday|";
            ExecuteAndAssert("SimplePayloadWithArrayProperty", expectedTable);
        }

        [Fact]
        public void ParseJsonWithInsideSimpleObjectSouldReturnAllItemsFlattedToValidTable()
        {
            var expectedTable = $"|Name|Basket.IsEmpty|Basket.IsFromRefound|{Environment.NewLine}|This is a test|True|False|";
            ExecuteAndAssert("SimplePayloadWithInsideObject", expectedTable);
        }

        [Fact]
        public void ParseJsonWithObjectAndArrayShouldReturnAllItemsFlattedToValidTable()
        {
            var expectedTable = $"|name|References.IsActive|References.Load|DaysToLoad|{Environment.NewLine}|This is a name|True|90|Monday,Whenesday,Friday,Saturday|";
            ExecuteAndAssert("SimplePayloadWithObjectAndArray", expectedTable);
        }

        [Fact]
        public void ParseComplexJsonWithArrayInsideObjectSouldReturnAllItemsFlattedToValidTable()
        {
            var expectedTable = $"|Name|Configurations.IsActive|Configurations.ScheduledDays|LastUpdate|{Environment.NewLine}"
                              + $"|ComplexPayload|True|Monday,Saturday,Sunday|10/10/2022|";
            ExecuteAndAssert("ComplexPayloadArrayInsideObject", expectedTable);
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

            ExecuteAndAssert("ComplexPayloadMultipleInsideArrayAndObject", expectedTable);
        }

        private static void ExecuteAndAssert(string payload, string expectedTable)
        {
            var sut = GetNewConverter().Convert(PayloadLoader.GetPayloadAsString(payload));
            AssertValidTable(expectedTable, sut);
        }

        private static void AssertValidTable(string ExpectedTableResult, string sut)
        {
            sut.Should().NotBeNullOrEmpty();
            sut.Should().BeEquivalentTo(ExpectedTableResult);
        }

        private static IJsonConverterToExampleTable GetNewConverter() => new JsonConverterToExampleTable();
    }
}
