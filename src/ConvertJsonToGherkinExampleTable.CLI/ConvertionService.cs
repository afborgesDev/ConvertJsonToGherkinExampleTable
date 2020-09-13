﻿using System;
using System.Collections.Generic;
using System.IO;
using ConvertJsonToGherkinExampleTable.Core;
using Microsoft.Extensions.Logging;
using TextCopy;

namespace ConvertJsonToGherkinExampleTable.CLI
{
    public class ConvertionService : IConvertionService
    {
        private readonly ILogger<ConvertionService> logger;
        private readonly IJsonConverterToExampleTable jsonConverterToExampleTable;
        private readonly IClipboard clipboard;

        public ConvertionService(ILogger<ConvertionService> logger, IJsonConverterToExampleTable jsonConverterToExampleTable, IClipboard clipboard)
        {
            this.logger = logger;
            this.jsonConverterToExampleTable = jsonConverterToExampleTable;
            this.clipboard = clipboard;
        }

        public void Convert(string? filePath, string? folderPath, string? destinationFolder)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                ProceedFileConvertion(filePath, destinationFolder);
                return;
            }

            if (!string.IsNullOrEmpty(folderPath))
                ProceedFolderConvertion(folderPath, destinationFolder);

            logger.LogError("Must have the filePath or the folderPath to execute");
        }

        private void ProceedFileConvertion(string filePath, string? destinationFolder)
        {
            if (!File.Exists(filePath))
            {
                logger.LogError($"Could not find the {filePath} to proceed the convertion.");
                return;
            }

            logger.LogInformation("Getting the payload");
            var payload = File.ReadAllText(filePath);
            logger.LogInformation("Convertiong");
            var convertionResult = jsonConverterToExampleTable.Convert(payload);
            SaveResult(destinationFolder, convertionResult);
        }

        private void ProceedFolderConvertion(string folderPath, string? destinationFolder)
        {
            if (!Directory.Exists(folderPath))
            {
                logger.LogError("The origin folder doesn't exist");
                return;
            }

            var payloads = new List<string>();
            foreach (var item in Directory.EnumerateFiles(folderPath, "*.json", SearchOption.TopDirectoryOnly))
                payloads.Add(File.ReadAllText(item));

            var convertionResult = jsonConverterToExampleTable.ConvertMultipleIntoSingleTable(payloads.ToArray());
            SaveResult(destinationFolder, convertionResult);
        }

        private void SaveResult(string? destinationFolder, string? convertionResult)
        {
            logger.LogInformation("Saving result");
            if (string.IsNullOrEmpty(convertionResult) ||
                convertionResult.Equals(JsonConverterToExampleTable.CouldNotConvertJsonIntoTableMessage, StringComparison.InvariantCultureIgnoreCase))
            {
                logger.LogError("Could not proceed the convertion please check out if the JSON is well formed");
                return;
            }

            if (!string.IsNullOrEmpty(destinationFolder))
            {
                File.WriteAllText(destinationFolder, convertionResult);
                return;
            }

            clipboard.SetText(convertionResult);
        }
    }
}
