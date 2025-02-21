namespace AspireDaprDemo.ServiceDefaults;

public sealed record WeatherForecastEvent(WeatherForecast[] Forecasts, string Message);

public sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(this.TemperatureC / 0.5556);
}

public sealed record CachedWeatherForecast(WeatherForecast[] Forecasts, DateTimeOffset CachedAt);

public sealed record WeatherForeCastMessagesRequested(int Count);
