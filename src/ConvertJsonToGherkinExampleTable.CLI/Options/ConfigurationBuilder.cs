using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace ConvertJsonToGherkinExampleTable.CLI.Options
{
    [ExcludeFromCodeCoverage]
    public class ConfigurationBuilder
    {
        private readonly ILogger<ConfigurationBuilder> logger;
        private string? FilePath;
        private string? FolderPath;
        private string? ResultFilePath;
        private bool? PayloadFromClipboard;
        private bool? GenCode;
        private string? DestGenCodeFolder;

        private ConfigurationBuilder(ILogger<ConfigurationBuilder> logger) => this.logger = logger;

        public static ConfigurationBuilder StartBuild(ILogger<ConfigurationBuilder> logger) => new ConfigurationBuilder(logger);

        public ConfigurationBuilder WithFilePath(string? filePath)
        {
            FilePath = filePath;
            return this;
        }

        public ConfigurationBuilder WithFolderPath(string? folderPath)
        {
            FolderPath = folderPath;
            return this;
        }

        public ConfigurationBuilder WithResultFilePath(string? resultFilePath)
        {
            ResultFilePath = resultFilePath;
            return this;
        }

        public ConfigurationBuilder WithPayloadFromClipboard(bool? payloadFromClipboard)
        {
            PayloadFromClipboard = payloadFromClipboard;
            return this;
        }

        public ConfigurationBuilder WithCodeGeneration(bool? genCode)
        {
            GenCode = genCode;
            return this;
        }

        public ConfigurationBuilder WithDestGenCodeFolder(string? destGencodeFolder)
        {
            DestGenCodeFolder = destGencodeFolder;
            return this;
        }

        public ConvertConfigurations? Build()
        {
            if (!Validate())
                return default;

            if (PayloadFromClipboard != null && PayloadFromClipboard == true)
            {
                FilePath = default;
                FolderPath = default;
            }

            return new ConvertConfigurations {
                FilePath = FilePath,
                FolderPath = FolderPath,
                ResultFilePath = ResultFilePath,
                PayloadFromClipboard = PayloadFromClipboard,
                GenerateCode = GenCode,
                DestinationGeneratedCode = DestGenCodeFolder
            };
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(FilePath) &&
                string.IsNullOrWhiteSpace(FolderPath) &&
                (PayloadFromClipboard is null || PayloadFromClipboard == false))
            {
                logger.LogError("Must have at the least one of those options: FilePath, FolderPath, or FromClipboard");
                return false;
            }

            if (GenCode != null && GenCode == true &&
                string.IsNullOrEmpty(DestGenCodeFolder))
            {
                logger.LogError("To generate code must have the destination folder");
                return false;
            }

            return true;
        }
    }
}
