using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using ConvertJsonToGherkinExampleTable.CLI.Options;
using ConvertJsonToGherkinExampleTable.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TextCopy;

namespace ConvertJsonToGherkinExampleTable.CLI
{
    [ExcludeFromCodeCoverage]
    internal class Program
    {
        private static void Main(string[] args) => BuildCommandLine()
                .UseHost(_ => Host.CreateDefaultBuilder()
                                  , host =>
                                  {
                                      host.ConfigureServices(services =>
                                      {
                                          services.InjectClipboard();
                                          services.AddSingleton<IJsonConverterToExampleTable, JsonConverterToExampleTable>();
                                          services.AddSingleton<IConvertionService, ConvertionService>();
                                      });
                                  })
                .UseDefaults()
                .Build()
                .Invoke(args);

        private static CommandLineBuilder BuildCommandLine()
        {
            var root = new RootCommand(@"$ convert the JSON into Example Gherking language"){
                new Option<string>(new string[]{"--jsonFile", "-j" }, "Indicate the complete path to the JSON file to be converted"),
                new Option<string>(new string[]{ "--jsonFolder", "-jp" }, "Indicate the folder to the JSON files to be converted"),
                new Option<string>(new string[]{"--destFolder", "-df" }, "the result folder, if this param won't provide the result will come to Clipboard"),
                new Option<bool>(new string[]{"--fromclp", "-fcp" }, "indicate that the payload source should come from the Clipboard"),
                new Option<bool>(new string[]{"--gencode","-gc" }, "Indicate that the code to use the table will be created"),
                new Option<string>(new string[]{"--destgencode", "-dsg" }, "Indicate the destin folder for the generated code")
            };
            root.Handler = CommandHandler.Create(
                (JsonFileOption jsonFileOption, JsonFolderOption jsonFolderOption, DestinationFolderOption destinationFolderOption,
                 FromClipboardOption fromClipboardOption, GenerateCodeOption genCodeOption, DestinationGenCodeOption destinationGenCodeOption, IHost host) =>
                {
                    var serviceProvider = host.Services;
                    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                    var config = ConfigurationBuilder.StartBuild(loggerFactory.CreateLogger<ConfigurationBuilder>())
                                                     .WithFilePath(jsonFileOption?.JsonFile)
                                                     .WithFolderPath(jsonFolderOption?.JsonFolder)
                                                     .WithResultFilePath(destinationFolderOption?.DestFolder)
                                                     .WithPayloadFromClipboard(fromClipboardOption?.FromClp)
                                                     .WithCodeGeneration(genCodeOption?.GenCode)
                                                     .WithDestGenCodeFolder(destinationGenCodeOption?.DestGenCode)
                                                     .Build();
                    Run(config, serviceProvider);
                });

            return new CommandLineBuilder(root);
        }

        private static void Run(ConvertConfigurations? convertConfigurations, IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();

            if (convertConfigurations is null)
            {
                logger.LogError("Theres some problems at the configurations, please check the entry params");
                return;
            }

            logger.LogInformation("Starting the process");
            var convertionService = serviceProvider.GetRequiredService<IConvertionService>();
            convertionService.Convert(convertConfigurations);
        }
    }
}
