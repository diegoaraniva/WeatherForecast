namespace Application.Dto.WeatherManagement.CurrentWeather;

public class WeatherResponseDTO
{
    public string Location { get; set; }
    public string Timestamp { get; set; }
    public float Temperature { get; set; }
    public float FeelsLike { get; set; }
    public float TempMin { get; set; }
    public float TempMax { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }
    public string WeatherMain { get; set; }
    public string WeatherDescription { get; set; }
    public float WindSpeed { get; set; }
    public int Cloudiness { get; set; }
    public string Icon { get; set; }
    
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdate { get; set; }
}