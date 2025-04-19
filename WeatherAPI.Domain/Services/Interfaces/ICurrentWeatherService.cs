using Domain.Dto.WeatherManagement.CurrentWeather;

namespace Domain.Services;

public interface ICurrentWeatherService
{
    Task<CurrentWeatherDTO> GetCurrentWeather();
}