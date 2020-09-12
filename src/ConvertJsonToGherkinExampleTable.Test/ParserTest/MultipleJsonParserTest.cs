using System;
using ConvertJsonToGherkinExampleTable.Core;
using Xunit;

namespace ConvertJsonToGherkinExampleTable.Test.ParserTest
{
    public class MultipleJsonParserTest
    {
        [Fact]
        public void ParseCoupleOfSimpleJsonShouldReturnValidTableWithMultiLines()
        {
            var expectedTable = $"|name|Age|{Environment.NewLine}|this is a test|33|{Environment.NewLine}|this is a test|33|";
            var firstJson = PayloadLoader.GetPayloadAsString("TwoItemsPayload");
            var SecondJson = PayloadLoader.GetPayloadAsString("TwoItemsPayload");
            var sut = JsonParserGeneralTest.GetNewConverter().ConvertMultipleIntoSingleTable(firstJson, SecondJson);
            JsonParserGeneralTest.AssertValidTable(expectedTable, sut);
        }

        [Fact]
        public void ParseEmptyJsonShouldReturnErrorMessage()
        {
            var sut = JsonParserGeneralTest.GetNewConverter().ConvertMultipleIntoSingleTable(string.Empty, string.Empty);
            JsonParserGeneralTest.AssertValidTable(JsonConverterToExampleTable.CouldNotConvertJsonIntoTableMessage, sut);
        }

        [Fact]
        public void ParseInvalidJsonShouldReturnErrorMessage()
        {
            var sut = JsonParserGeneralTest.GetNewConverter().ConvertMultipleIntoSingleTable("{-01", "}{}");
            JsonParserGeneralTest.AssertValidTable(JsonConverterToExampleTable.CouldNotConvertJsonIntoTableMessage, sut);
        }

        [Fact]
        public void ParseMultiDifferentJsonShouldReturnErrorMessage()
        {
            var firstJson = PayloadLoader.GetPayloadAsString("TwoItemsPayload");
            var secondJson = PayloadLoader.GetPayloadAsString("SimplePayloadWithInsideObject");
            var sut = JsonParserGeneralTest.GetNewConverter().ConvertMultipleIntoSingleTable(firstJson, secondJson);
            JsonParserGeneralTest.AssertValidTable(JsonConverterToExampleTable.AllJsonsShouldHaveSameFieldNamsToConvertMultipleIntoOneTable, sut);
        }
    }
}
