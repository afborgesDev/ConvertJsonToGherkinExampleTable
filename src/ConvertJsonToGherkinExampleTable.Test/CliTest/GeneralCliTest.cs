using System;
using System.IO;
using System.Linq;
using ConvertJsonToGherkinExampleTable.CLI;
using ConvertJsonToGherkinExampleTable.Core;
using ConvertJsonToGherkinExampleTable.Test.ParserTest;
using CrawlerWave.LogTestHelper;
using CrawlerWave.LogTestHelper.Configurations;
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

        [Fact]
        public void ConvertWithInvalidFileShouldLogErrorAndDontGenerateOutput()
        {
            var (sink, convertionService) = CreateServiceWithSink();

            var invalidFilePath = $"_invalid_{Guid.NewGuid()}.json";
            var destFilePath = $"destFile_{Guid.NewGuid()}.txt";

            convertionService.Convert(invalidFilePath, string.Empty, destFilePath);
            File.Exists(destFilePath).Should().BeFalse();
            AssertLogMessage(sink, $"Could not find the {invalidFilePath} to proceed the convertion.", LogLevel.Error);
        }

        [Fact]
        public void ConvertWithInvalidDirectoryShouldLogErrorAndDontGEnerateOutput()
        {
            var (sink, convertionService) = CreateServiceWithSink();

            var invalidfolder = $"_invalid_{Guid.NewGuid()}";
            var destFilePath = $"destFile_{Guid.NewGuid()}.txt";

            convertionService.Convert(string.Empty, invalidfolder, destFilePath);
            File.Exists(destFilePath).Should().BeFalse();
            AssertLogMessage(sink, "The origin folder doesn't exist", LogLevel.Error);
        }

        [Fact]
        public void BadFormatedInputShouldNotConvert()
        {
            var (sink, convertionService) = CreateServiceWithSink();

            var destFilePath = $"destFile_{Guid.NewGuid()}.txt";
            var invalidFile = $"_invalid_{Guid.NewGuid()}.json";
            File.WriteAllText(invalidFile, "invalid");
            try
            {
                convertionService.Convert(invalidFile, string.Empty, destFilePath);
                File.Exists(destFilePath).Should().BeFalse();
                AssertLogMessage(sink, "Could not proceed the convertion please check out if the JSON is well formed", LogLevel.Error);
            }
            finally
            {
                File.Delete(invalidFile);
            }
        }

        private static (ITestSink, ConvertionService) CreateServiceWithSink()
        {
            var (sink, logFactory) = LogTestHelperInitialization.Create();
            var mockLogger = logFactory.CreateLogger<ConvertionService>();
            var jsonConverter = new JsonConverterToExampleTable();
            var clipboard = new Mock<Clipboard>();
            var convertionService = new ConvertionService(mockLogger, jsonConverter, clipboard.Object);
            return (sink, convertionService);
        }

        private static ConvertionService CreateService()
        {
            var mockLogger = new Mock<ILogger<ConvertionService>>();
            var jsonConverter = new JsonConverterToExampleTable();
            var clipBoard = new Clipboard();
            return new ConvertionService(mockLogger.Object, jsonConverter, clipBoard);
        }

        private static void AssertLogMessage(ITestSink sink, string logMesage, LogLevel logLevel) =>
            sink.Writes.Any(x => x.Message.Contains(logMesage, StringComparison.InvariantCultureIgnoreCase) && x.LogLevel == logLevel)
                       .Should().BeTrue();
    }
}
