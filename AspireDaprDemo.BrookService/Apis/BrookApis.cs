using AspireDaprDemo.ServiceDefaults;
using Azure.Data.Tables;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace AspireDaprDemo.BrookService.Apis
{
    public static class BrookApis
    {
        public static void MapEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/event", [Topic("pubsub", "weather")] async ([FromServices] DaprClient daprclient, [FromServices] TableServiceClient tableServiceClient, [FromBody] WeatherForecastEvent @event) =>
            {
                Console.WriteLine($"Received event {@event.Message}");

                tableServiceClient.CreateTableIfNotExists("messages");
                var tableClient = tableServiceClient.GetTableClient("messages");

                tableClient.AddEntity(new TableEntity("Messages", Guid.NewGuid().ToString())
                {
                    {"Message", @event.Message + " Forecasts " + @event.Forecasts.Length}
                });

                return Results.Ok();
            });
        }
    }
}
