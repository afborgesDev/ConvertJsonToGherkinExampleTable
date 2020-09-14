using System.Diagnostics.CodeAnalysis;

namespace ConvertJsonToGherkinExampleTable.CLI.Options
{
    [ExcludeFromCodeCoverage]
    public class ConfigurationBuilder
    {
        private string? FilePath;
        private string? FolderPath;
        private string? ResultFilePath;
        private bool? PayloadFromClipboard;

        public static ConfigurationBuilder StartBuild() => new ConfigurationBuilder();

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

        public ConvertConfigurations Build()
        {
            if (PayloadFromClipboard != null && PayloadFromClipboard == true)
            {
                FilePath = default;
                FolderPath = default;
            }

            return new ConvertConfigurations {
                FilePath = FilePath,
                FolderPath = FolderPath,
                ResultFilePath = ResultFilePath,
                PayloadFromClipboard = PayloadFromClipboard
            };
        }
    }
}
