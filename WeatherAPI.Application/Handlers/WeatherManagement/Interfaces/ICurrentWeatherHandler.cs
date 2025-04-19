using Application.Dto.WeatherManagement.CurrentWeather;

namespace Application.Handlers.WeatherManagement.Interfaces;

public interface ICurrentWeatherHandler
{
    Task<WeatherResponseDTO> GetCurrentWeather();
}