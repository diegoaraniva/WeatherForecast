namespace Domain.Dto.WeatherManagement.ForecastWeather;

public class ForecastWeatherDTO
{
    public string Cod { get; set; }
    public int Message { get; set; }
    public int Cnt { get; set; }
    public List<SingleForecastDTO> List { get; set; }
}