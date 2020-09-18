dotnet build .\src\ConvertJsonToGherkinExampleTable.sln
dotnet pack .\src\ConvertJsonToGherkinExampleTable.CLI\ConvertJsonToGherkinExampleTable.CLI.csproj
dotnet tool install --global --add-source .\src\ConvertJsonToGherkinExampleTable.CLI\nupkg ConvertJsonToGherkinExampleTable.CLI
convertJsToGh -h