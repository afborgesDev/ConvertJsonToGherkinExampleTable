using System;
using System.IO;
using System.Linq;
using ConvertJsonToGherkinExampleTable.CLI;
using ConvertJsonToGherkinExampleTable.CLI.Options;
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
            var config = ConfigurationBuilder.StartBuild(CreateLoggerForConfigurationBuilder())
                                             .WithFilePath($"example_{Guid.NewGuid()}.json")
                                             .WithResultFilePath($"destFile_{Guid.NewGuid()}.txt")
                                             .Build();
            try
            {
                File.WriteAllText(config.FilePath, jsonToTest);
                sut.Convert(config);
                var result = File.ReadAllText(config.ResultFilePath);
                result.Should().BeEquivalentTo(JsonParserGeneralTest.ExpectedTableTwoItemsPayload);
            }
            finally
            {
                CleanUpTestScenario(config);
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
            var destFilePath = $"destFile_{Guid.NewGuid()}.txt";
            var file1 = Path.Combine(originFolder, $"example1_{Guid.NewGuid()}.json");
            var file2 = Path.Combine(originFolder, $"example2_{Guid.NewGuid()}.json");

            var config = ConfigurationBuilder.StartBuild(CreateLoggerForConfigurationBuilder())
                                             .WithFolderPath(originFolder)
                                             .WithResultFilePath(destFilePath)
                                             .Build();

            try
            {
                File.WriteAllText(file1, jsonToTest);
                File.WriteAllText(file2, jsonToTest);

                sut.Convert(config);
                var result = File.ReadAllText(destFilePath);
                result.Should().BeEquivalentTo(expected);
            }
            finally
            {
                CleanUpTestScenario(config);
            }
        }

        [Fact]
        public void ConvertWithInvalidFileShouldLogErrorAndDontGenerateOutput()
        {
            var (sink, convertionService) = CreateServiceWithSink();
            var config = ConfigurationBuilder.StartBuild(CreateLoggerForConfigurationBuilder())
                                             .WithFilePath($"_invalid_{Guid.NewGuid()}.json")
                                             .WithResultFilePath($"destFile_{Guid.NewGuid()}.txt")
                                             .Build();

            convertionService.Convert(config);
            File.Exists(config.ResultFilePath).Should().BeFalse();
            AssertLogMessage(sink, $"Could not find the {config.FilePath} to proceed the convertion.", LogLevel.Error);
        }

        [Fact]
        public void ConvertWithInvalidDirectoryShouldLogErrorAndDontGEnerateOutput()
        {
            var (sink, convertionService) = CreateServiceWithSink();
            var config = ConfigurationBuilder.StartBuild(CreateLoggerForConfigurationBuilder())
                                             .WithFolderPath($"_invalid_{Guid.NewGuid()}")
                                             .WithResultFilePath($"destFile_{Guid.NewGuid()}.txt")
                                             .Build();

            convertionService.Convert(config);
            File.Exists(config.ResultFilePath).Should().BeFalse();
            AssertLogMessage(sink, "The origin folder doesn't exist", LogLevel.Error);
        }

        [Fact]
        public void BadFormatedInputShouldNotConvert()
        {
            var (sink, convertionService) = CreateServiceWithSink();
            var config = ConfigurationBuilder.StartBuild(CreateLoggerForConfigurationBuilder())
                                             .WithFilePath($"_invalid_{Guid.NewGuid()}.json")
                                             .WithResultFilePath($"destFile_{Guid.NewGuid()}.txt")
                                             .Build();
            File.WriteAllText(config.FilePath, "invalid");
            try
            {
                convertionService.Convert(config);
                File.Exists(config.ResultFilePath).Should().BeFalse();
                AssertLogMessage(sink, "Could not proceed the convertion please check out if the JSON is well formed", LogLevel.Error);
            }
            finally
            {
                CleanUpTestScenario(config);
            }
        }

        [SkippableFact]
        public void ConvertFromValidPayloadOnClipboardShouldGenerateValideExampleTable()
        {
            Skip.If(Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX);

            var simplePayload = PayloadLoader.GetPayloadAsString("TwoItemsPayload");
            new Clipboard().SetText(simplePayload);
            var convertionService = CreateService();
            var config = ConfigurationBuilder.StartBuild(CreateLoggerForConfigurationBuilder())
                                             .WithPayloadFromClipboard(true)
                                             .Build();
            convertionService.Convert(config);
            var result = new Clipboard().GetText();
            result.Should().BeEquivalentTo(JsonParserGeneralTest.ExpectedTableTwoItemsPayload);
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

        private static ILogger<ConfigurationBuilder> CreateLoggerForConfigurationBuilder() =>
            new Mock<ILogger<ConfigurationBuilder>>().Object;

        private static void AssertLogMessage(ITestSink sink, string logMesage, LogLevel logLevel) =>
            sink.Writes.Any(x => x.Message.Contains(logMesage, StringComparison.InvariantCultureIgnoreCase) && x.LogLevel == logLevel)
                       .Should().BeTrue();

        private static void CleanUpTestScenario(ConvertConfigurations configurations)
        {
            if (!string.IsNullOrEmpty(configurations.FilePath))
                File.Delete(configurations.FilePath);

            if (!string.IsNullOrEmpty(configurations.ResultFilePath))
                File.Delete(configurations.ResultFilePath);

            if (!string.IsNullOrEmpty(configurations.FolderPath))
                Directory.Delete(configurations.FolderPath, true);
        }
    }
}
