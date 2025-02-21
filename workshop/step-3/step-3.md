# Opdracht Implementeer eventing mbv Pubsub

In deze opdracht gaan we pubsub implementeren die bij Dapr kan worden toegepast we gaan de weatherforecasts publishen om deze later in een table storage te publishen.

### Stap 3.1)

De eerste stap is het aanmaken van een pubsub mogelijkheid dit is vrijwel vergelijkbaar met het aanmaken van een state component in het apphost project. Probeer zelf een pubsub component aan te maken met de naam `pubsub` refereer dit onderdeel vervolgens aan de `bob` service.

Voeg vervolgens aan de Bob service een command toe die middels een event published gebruik voor het bericht wat verstuurd moeten worden de class `WeatherForecastEvent`

```c#
PublishEventAsync(...)
```

### Stap 3.2

De volgende stap is dat we het event ontvangen en daarmee aan de slag gaan om het te verwerken. Dit verwerken doen we in de `Brooks` Service. De volgende stappen moeten worden onderdnomen.

- Voeg het pubsub component toe in de AppHost.
- Registreer de CloudEvents
- Maak een endpoint om de events te ontvangen

**Registreer Cloud Events**

Voeg de onderstaande code toe aan de juiste service voor het ontvangen van het bericht.

```c#
app.UseCloudEvents();
app.MapSubscribeHandler(); // mapped events aan de post controllers van de verschillende api's
```

In de Brookservice kan de volgende api worden toegevoegd

```c#
routes.MapPost("/event", [Topic("...", "...")] async ([FromServices] DaprClient daprclient, [FromBody] WeatherForecastEvent @event) =>
```

breidt de methode uit om bijvoorbeeld de berichten in de state op te slaan.


