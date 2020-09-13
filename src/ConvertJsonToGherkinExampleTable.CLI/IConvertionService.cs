namespace ConvertJsonToGherkinExampleTable.CLI
{
    public interface IConvertionService
    {
        void Convert(string? filePath, string? folderPath, string? destinationFolder);
    }
}
