using Application.Dto.WeatherManagement.CurrentWeather;
using Application.Dto.WeatherManagement.ForecastWeather;
using Application.Handlers.WeatherManagement.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using TechnicalTest.Controllers;

namespace UnitTesting.IntegralTestings;

public class ConsumingWebService
{
    private readonly HttpClient _client;
    private readonly Mock<ICurrentWeatherHandler> _mockWeather;
    private readonly Mock<IForecastWeatherHandler> _mockForecastWeather;
    private readonly Mock<ILogger<WeatherController>> _mockLogger;

    public ConsumingWebService()
    {
        var mockedWeather = new WeatherResponseDTO();
        var mockedForecast = new List<ForecastResponseDTO>();
        
        _mockWeather = new Mock<ICurrentWeatherHandler>();
        _mockWeather.Setup(service => service.GetCurrentWeather())
            .ReturnsAsync(mockedWeather);
        
        _mockForecastWeather = new Mock<IForecastWeatherHandler>();
        _mockForecastWeather.Setup(service => service.GetForecast())
            .ReturnsAsync(mockedForecast);
        _mockLogger = new Mock<ILogger<WeatherController>>();
        
        var webHostBuilder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddRouting();
                services.AddControllers();  
                services.AddSingleton(_mockWeather.Object);
                services.AddSingleton(_mockForecastWeather.Object);
                services.AddSingleton(_mockLogger.Object);
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });

        var server = new TestServer(webHostBuilder);
        _client = server.CreateClient();
    }
    
    [Fact]
    public async Task ReturnsMockedWeather()
    {
        var response = await _client.GetAsync("/current");
        
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var weatherResponse = JsonConvert.DeserializeObject<WeatherResponseDTO>(content);
        Assert.Equal(new WeatherResponseDTO(), weatherResponse);
    }
    
    [Fact]
    public async Task ReturnsMockedForecast()
    {
        var response = await _client.GetAsync("/forecast");
        
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var forecastResponse = JsonConvert.DeserializeObject<List<ForecastResponseDTO>>(content);
        Assert.Equal(new List<ForecastResponseDTO>(), forecastResponse);
    }
}