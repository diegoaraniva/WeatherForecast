using Domain;
using Microsoft.Extensions.DependencyInjection;
using Domain.Services;

namespace UnitTesting;

public class ExternalEndpointsTest
{
    private readonly ICurrentWeatherService _serviceWeather;
    private readonly IForecastService _forecastService;

    public ExternalEndpointsTest()
    {
        var services = new ServiceCollection();

        services.AddHttpClient();
        services.AddMemoryCache();
        
        services.AddSingleton<CircuitBreakerPolicyProvider>();

        services.AddScoped<ICurrentWeatherService, CurrentWeatherService>();
        services.AddScoped<IForecastService, ForecastService>();

        var provider = services.BuildServiceProvider();

        _serviceWeather = provider.GetRequiredService<ICurrentWeatherService>();
        _forecastService = provider.GetRequiredService<IForecastService>();
    }

    
    [Fact]
    public async Task ConsumeEndpointWeather()
    {
        var result = await _serviceWeather.GetCurrentWeather();
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task ConsumeEndpointForecast()
    {
        var result = await _forecastService.GetForecast();
        Assert.NotNull(result);
    }
}