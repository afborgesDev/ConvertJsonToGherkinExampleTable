[![codecov](https://codecov.io/gh/afborgesDev/ConvertJsonToGherkinExampleTable/branch/main/graph/badge.svg)](https://codecov.io/gh/afborgesDev/ConvertJsonToGherkinExampleTable)

# Supported type of Json Example:

- Simple:  
Payload:  
```json
{
  "Name": "this is a test",
  "Age": 33
}
```

Expected Table:  
```gherkin
| Name           | Age |
| this is a test | 33  |
```
---
- Simple with array property:  
Payload:  
```json
{
  "Name": "this is a test",
  "DaysPerWeekWorkOut": [ "Sunday", "Wendsday", "Friday" ]
}
```
Expected Table:  
```gherkin
| Name           | DaysPerWeekWorkOut      |
| this is a test | Sunday,Wendsday,Friday  |
```
---
- Simple with object property:  
Payload: 
```json
{
  "Name": "This is a test",
  "Basket": {
    "IsEmpty": true,
    "IsFromRefound": false
  }
}
```
Expected Table:  
```gherkin
| Name           | Basket.IsEmpty | Basket.IsFromRefound |
| this is a test | True           | False                |
```
---
- Simple with array and object property:  
Payload: 
```json
{
  "Name": "This is a name",
  "References": {
    "IsActive": true,
    "Load": 90
  },
  "DaysToLoad": [
    "Monday",
    "Whenesday",
    "Friday",
    "Saturday"
  ]
}
```
Expected Table:  
```gherkin
| Name           | References.IsActive | References.Load | DaysToLoad                       |
| this is a test | True                | 90              | Monday,Whenesday,Friday,Saturday |
```
---
- Complex with array inside object property:  
Payload:  
```json
{
  "Name": "ComplexPayload",
  "Configurations": {
    "IsActive": true,
    "ScheduledDays": [
      "Monday",
      "Saturday",
      "Sunday"
    ]
  },
  "LastUpdate": "10/10/2022"
}
```
Expected Table:  
```gherkin
| Name           | Configurations.IsActive | Configurations.ScheduledDays | LastUpdate |
| ComplexPayload | True                    | Monday,Saturday,Sunday       | 10/10/2022 |
```
---
- Complex with array, object inside object property:  
Payload:  
```json
{
  "Name": "ComplexPayload",
  "Configurations": {
    "IsActive": true,
    "ScheduledDays": [
      "Monday",
      "Saturday",
      "Sunday"
    ]
  },
  "LastUpdate": "10/10/2022",
  "UpdateInfo": {
    "Owner": {
      "FirstName": "User",
      "LastName": "Beta",
      "IsAdmin": false
    },
    "PipeUpdated": {
      "Count": 2,
      "HasSomeError": false,
      "ExecutionDetail": {
        "Log": [
          "Succeed",
          "Succeed"
        ]
      }
    }
  }
}
```
Expected Table:  
```gherkin
| Name           | Configurations.IsActive | Configurations.ScheduledDays | LastUpdate | UpdateInfo.Owner.FirstName | UpdateInfo.Owner.LastName | UpdateInfo.Owner.IsAdmin | UpdateInfo.PipeUpdated.Count | UpdateInfo.PipeUpdated.HasSomeError | UpdateInfo.PipeUpdated.ExecutionDetail.Log |
| ComplexPayload | True                    | Monday,Saturday,Sunday       | 10/10/2022 | User                       | Beta                      | False                    | 2                            | False                               | Succeed,Succeed                            |
```
