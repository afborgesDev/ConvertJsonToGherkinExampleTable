using System;
using ConvertJsonToGherkinExampleTable.Core;
using FluentAssertions;
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
            var sut = new JsonConverterToExampleTable().ConvertMultipleIntoSingleTable(firstJson, SecondJson);
            sut.Should().NotBeNullOrEmpty();
            sut.Should().BeEquivalentTo(expectedTable);
        }

        /*
         Corner cases:
        1) Empty
        2) different
        3) invalid
        */
    }
}
