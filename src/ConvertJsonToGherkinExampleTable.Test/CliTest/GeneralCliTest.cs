using System;
using System.IO;
using ConvertJsonToGherkinExampleTable.CLI;
using ConvertJsonToGherkinExampleTable.Core;
using ConvertJsonToGherkinExampleTable.Test.ParserTest;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TextCopy;
using Xunit;

namespace ConvertJsonToGherkinExampleTable.Test.CliTest
{
    public class GeneralCliTest
    {
        [Fact]
        public void ConvertSingleValidJsonShouldReturnExampleTableToFileDest()
        {
            var sut = CreateService();
            var jsonToTest = PayloadLoader.GetPayloadAsString("TwoItemsPayload");
            var filePathParam = $"example_{Guid.NewGuid()}.json";
            var destFilePath = $"destFile_{Guid.NewGuid()}.txt";
            try
            {
                File.WriteAllText(filePathParam, jsonToTest);
                sut.Convert(filePathParam, default, destFilePath);
                var result = File.ReadAllText(destFilePath);
                result.Should().BeEquivalentTo(JsonParserGeneralTest.ExpectedTableTwoItemsPayload);
            }
            finally
            {
                File.Delete(filePathParam);
                File.Delete(destFilePath);
            }
        }

        [Fact]
        public void ConvertMultipleValidJsonShouldReturnMultilineExampleTableToFileDest()
        {
            var expected = $"|name|Age|{Environment.NewLine}|this is a test|33|{Environment.NewLine}|this is a test|33|";
            var sut = CreateService();
            var jsonToTest = PayloadLoader.GetPayloadAsString("TwoItemsPayload");

            var fileFolderGuid = Guid.NewGuid().ToString();
            var originFolder = Path.Combine(Directory.GetCurrentDirectory(), fileFolderGuid);
            Directory.CreateDirectory(originFolder);

            var file1 = Path.Combine(originFolder, $"example1_{Guid.NewGuid()}.json");
            var file2 = Path.Combine(originFolder, $"example2_{Guid.NewGuid()}.json");
            var destFilePath = $"destFile_{Guid.NewGuid()}.txt";
            try
            {
                File.WriteAllText(file1, jsonToTest);
                File.WriteAllText(file2, jsonToTest);

                sut.Convert(default, originFolder, destFilePath);
                var result = File.ReadAllText(destFilePath);
                result.Should().BeEquivalentTo(expected);
            }
            finally
            {
                Directory.Delete(originFolder, true);
                File.Delete(destFilePath);
            }
        }

        private static ConvertionService CreateService()
        {
            var mockLogger = new Mock<ILogger<ConvertionService>>();
            var jsonConverter = new JsonConverterToExampleTable();
            var clipBoard = new Clipboard();
            return new ConvertionService(mockLogger.Object, jsonConverter, clipBoard);
        }
    }
}
