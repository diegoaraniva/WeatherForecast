using Domain.Dto.WeatherManagement.CurrentWeather;
using Domain.Dto.WeatherManagement.ForecastWeather;
using Domain.Services;
using Moq;

namespace UnitTesting;

public class ExternalEndpointsTest
{
    private readonly Mock<ICurrentWeatherService> _serviceWeatherMock;
    private readonly Mock<IForecastService> _forecastServiceMock;

    public ExternalEndpointsTest()
    {
        _serviceWeatherMock = new Mock<ICurrentWeatherService>();
        _forecastServiceMock = new Mock<IForecastService>();
    }
    
    [Fact]
    public async Task ConsumeEndpointWeather()
    {
        var weatherResult = new CurrentWeatherDTO();
        _serviceWeatherMock
            .Setup(x => x.GetCurrentWeather())
            .ReturnsAsync(weatherResult);
        
        var result = await _serviceWeatherMock.Object.GetCurrentWeather();
        
        Assert.NotNull(result);
        _serviceWeatherMock.Verify(x => x.GetCurrentWeather(), Times.Once);
    }
    
    [Fact]
    public async Task ConsumeEndpointForecast()
    {
        var forecastResult = new ForecastWeatherDTO();
        _forecastServiceMock
            .Setup(x => x.GetForecast())
            .ReturnsAsync(forecastResult);
        
        var result = await _forecastServiceMock.Object.GetForecast();
        
        Assert.NotNull(result);
        _forecastServiceMock.Verify(x => x.GetForecast(), Times.Once);
    }
}