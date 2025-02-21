using AspireDaprDemo.ServiceDefaults;
using Dapr.Client;

namespace AspireDaprDemo.BobService.Apis
{
    public static class BobApis
    {
        private static readonly string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];
        public static void MapEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/weatherforecast", async (DaprClient daprClient) =>
            {

                // Simulating a flaky API in order to demonstrate Dapr's resilience features
                if (Random.Shared.NextDouble() > 0.4)
                {
                    throw new InvalidOperationException("Something wrong happened");
                }

                var forecasts = Enumerable.Range(1, 5)
                    .Select(index => new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.UtcNow.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();

                await daprClient.PublishEventAsync("pubsub", "weather", new WeatherForecastEvent(forecasts, "Weather forecast requested!"));

                return forecasts;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();
        }
    }
}
