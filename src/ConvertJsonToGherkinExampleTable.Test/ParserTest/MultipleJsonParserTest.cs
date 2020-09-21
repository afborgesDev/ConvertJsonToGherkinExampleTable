using System;
using ConvertJsonToGherkinExampleTable.Test.Common;
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
            var sut = JsonParserGeneralTest.GetNewConverter().ConvertMultipleIntoSingleTable(false, firstJson, SecondJson);
            CommonTestsHelper.AssertValidTable(expectedTable, sut);
        }

        [Fact]
        public void ParseEmptyJsonShouldReturnErrorMessage()
        {
            var sut = JsonParserGeneralTest.GetNewConverter().ConvertMultipleIntoSingleTable(false, string.Empty, string.Empty);
            CommonTestsHelper.AssertValidTable(GeneralConstants.CouldNotConvertJsonIntoTableMessage, sut);
        }

        [Fact]
        public void ParseInvalidJsonShouldReturnErrorMessage()
        {
            var sut = JsonParserGeneralTest.GetNewConverter().ConvertMultipleIntoSingleTable(false, "{-01", "}{}");
            CommonTestsHelper.AssertValidTable(GeneralConstants.CouldNotConvertJsonIntoTableMessage, sut);
        }

        [Fact]
        public void ParseMultiDifferentJsonShouldReturnErrorMessage()
        {
            var firstJson = PayloadLoader.GetPayloadAsString("TwoItemsPayload");
            var secondJson = PayloadLoader.GetPayloadAsString("SimplePayloadWithInsideObject");
            var sut = JsonParserGeneralTest.GetNewConverter().ConvertMultipleIntoSingleTable(false, firstJson, secondJson);
            CommonTestsHelper.AssertValidTable(GeneralConstants.AllJsonsShouldHaveSameFieldNamsToConvertMultipleIntoOneTable, sut);
        }
    }
}
