![Pipeline](https://github.com/afborgesDev/ConvertJsonToGherkinExampleTable/workflows/.NET%20Core/badge.svg)  
![CodeQL](https://github.com/afborgesDev/ConvertJsonToGherkinExampleTable/workflows/CodeQL/badge.svg)  
[![codecov](https://codecov.io/gh/afborgesDev/ConvertJsonToGherkinExampleTable/branch/main/graph/badge.svg)](https://codecov.io/gh/afborgesDev/ConvertJsonToGherkinExampleTable)  
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=afborgesDev_ConvertJsonToGherkinExampleTable&metric=alert_status)](https://sonarcloud.io/dashboard?id=afborgesDev_ConvertJsonToGherkinExampleTable)  
[![Nuget](https://img.shields.io/nuget/v/ConvertJsonToGherkinExampleTable.CLI)](https://www.nuget.org/packages/ConvertJsonToGherkinExampleTable.CLI/)

Capabilities:
  - Convert a single JSON into a single line table
  - Convert a couple JSON into a multi line table
    - All JSON must have the same fields number and name
  - Convert a JSON from clipboard

Usage:
```bash
dotnet tool install --global ConvertJsonToGherkinExampleTable.CLI --version 1.0.0
```

```bash
convertjstogh -h
```

To see examples of supported JSON and general documentation go to [Wiki](https://github.com/afborgesDev/ConvertJsonToGherkinExampleTable/wiki/)
