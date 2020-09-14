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
                new Option<string>(new string[]{"--jsonFile", "--jfl" }, "Indicate the complete path to the JSON file to be converted"),
                new Option<string>(new string[]{ "--jsonFolder", "--jfr" }, "Indicate the folder to the JSON files to be converted"),
                new Option<string>("--destFolder", "the result folder, if this param won't provide the result will come to Clipboard"),
                new Option<bool>(new string[]{"--fromclp", "--fclp" }, "indicate that the payload source should come from the Clipboard")
            };
            root.Handler = CommandHandler.Create<JsonFileOption, JsonFolderOption, DestinationFolderOption, FromClipboardOption, IHost>(Run);
            return new CommandLineBuilder(root);
        }

        private static void Run(JsonFileOption jsonFileOption, JsonFolderOption jsonFolderOption,
            DestinationFolderOption destinationFolderOption, FromClipboardOption fromClipboardOption, IHost host)
        {
            var serviceProvider = host.Services;
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(typeof(Program));
            logger.LogInformation("Starting the process");
            var convertionService = serviceProvider.GetRequiredService<IConvertionService>();
            convertionService.Convert(jsonFileOption?.JsonFile, jsonFolderOption?.JsonFolder, destinationFolderOption?.DestFolder);
        }
    }
}
