using AspireDaprDemo.ServiceDefaults;
using Dapr.Client;

namespace AspireDaprDemo.AliceService.Apis
{
    public static class AliceApis
    {
        public static void MapEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/weatherforecast", async (DaprClient daprclient) =>
            {
                // Attempt to get the cached weather forecast from Dapr state store
                var cachedForecasts = await daprclient.GetStateAsync<CachedWeatherForecast>("statestore", "cache");

                if (cachedForecasts is not null && cachedForecasts.CachedAt > DateTimeOffset.UtcNow.AddMinutes(-1))
                {
                    return cachedForecasts.Forecasts;
                }

                // Otherwise, get a fresh weather forecast from the flaky service "bob" and cache it
                var forecasts = await daprclient.InvokeMethodAsync<WeatherForecast[]>(HttpMethod.Get, "bob", "weatherforecast");

                await daprclient.SaveStateAsync("statestore", "cache", new CachedWeatherForecast(forecasts, DateTimeOffset.UtcNow));

                return forecasts;
            });
        }
    }
}
