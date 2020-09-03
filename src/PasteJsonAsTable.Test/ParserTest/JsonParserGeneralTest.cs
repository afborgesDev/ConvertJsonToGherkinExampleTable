using FluentAssertions;
using PasteJsonAsTable.Core.JsonParser;
using System;
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
            sut.Should().NotBeNullOrEmpty();
            sut.Should().Be(ExpectedTableResult);
        }
    }
}