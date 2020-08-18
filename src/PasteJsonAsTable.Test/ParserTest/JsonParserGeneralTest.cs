using FluentAssertions;
using PasteJsonAsTable.JsonParser;
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
    }
}