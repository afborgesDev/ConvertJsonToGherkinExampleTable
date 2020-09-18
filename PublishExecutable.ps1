Remove-Item -Path .\release -Recurse -Force -ErrorAction SilentlyContinue

dotnet publish -c Release -o .\release\win64 -r win-x64 --self-contained true .\src\ConvertJsonToGherkinExampleTable.CLI\ConvertJsonToGherkinExampleTable.CLI.csproj
dotnet publish -c Release -o .\release\linux -r linux-x64 --self-contained false .\src\ConvertJsonToGherkinExampleTable.CLI\ConvertJsonToGherkinExampleTable.CLI.csproj
dotnet publish -c Release -o .\release\macOs -r osx-x64 --self-contained true .\src\ConvertJsonToGherkinExampleTable.CLI\ConvertJsonToGherkinExampleTable.CLI.csproj