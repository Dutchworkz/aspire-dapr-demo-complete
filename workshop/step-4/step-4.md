# Opdracht Schrijf informatie naar de tablestorage

Het doel van deze opdracht is om de event informatie op te slaan in de table storage van Azure. Daarnaast willen we dit tonen in de Front-End applicatie.

### Opdracht 4.1)
Voeg table stotage toe aan de applicatie. 

```c#
var storage = builder.AddAzureStorage("storage");
var tables = storage.AddTables("tables");
```

Als extra toevoeging kun je ook gebruik maken van de Emulator.

```c#
if (!builder.ExecutionContext.IsPublishMode)
{
    storage.RunAsEmulator();
}
```
Mocht je nu denken nee ik wil dit graag zien gebeuren in Azure kun je ook een Azure koppeling in je Appsettings.development.json plaatsen van je apphost project dit zou er als volgt uit kunnen zien

```json
 "Logging": {
   "LogLevel": {
     "Default": "Information",
     "Microsoft.AspNetCore": "Warning"
   }
 },
 "Azure": {
   "SubscriptionId": "<<redacted>>",
   "AllowResourceGroupCreation": true,
   "ResourceGroup": "rg-aspire-001",
   "Location": "WestEurope",
   "CredentialSource": "AzureCli"
 }
```

### Opdracht 4.2)
Voeg de table storage toe aan de brook service. Let op dat je de Aspire Nuget package gebruikt en als het goed is heb je ook iets in het apphost project aangepast. Het volgende laat de koppeling zien in de brookservice

```c#
builder.AddAzureTableClient("tables");
```

Ook nog een klein stukje code wat je kunt bouwen bij het verwerken van het event.

```c#
 tableServiceClient.CreateTableIfNotExists("messages");
 var tableClient = tableServiceClient.GetTableClient("messages");

 tableClient.AddEntity(new TableEntity("Messages", Guid.NewGuid().ToString())
 {
     {"Message", @event.Message + " Forecasts " + @event.Forecasts.Length}
 });
```

Als je de applicatie nu start en wat events genereerd zou je de mogelijkheid moeten hebben om middels de storage explorer wat records te zien in de container

### Opdracht 4.3)

De volgende stap is om de resultaten ook zichtbaar te maken in de Front-End nu hebben jullie al de nodige ervaring op gedaan dus het enkele stukje code wat ik geef is het volgende

```c#
@inject TableServiceClient tableServiceClient;
```
en
```C#
......

protected override async Task OnInitializedAsync()
{
    // Simulate asynchronous loading to demonstrate streaming rendering
    await Task.Delay(500);

    var tableClient = tableServiceClient.GetTableClient("...");
    entities = tableClient.Query<TableEntity>().OrderByDescending(x => x.Timestamp).ToList();
}
```