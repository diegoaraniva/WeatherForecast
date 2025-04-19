namespace Domain.Dto.WeatherManagement.CurrentWeather;

public class CurrentWeatherDTO
{
    public CoordenatesDTO Coord { get; set; }
    public List<WeatherDTO> Weather { get; set; }
    public string Base { get; set; }
    public MainWeatherDTO Main { get; set; }
    public string Visibility { get; set; }
    public WindDTO Wind { get; set; }
    public CloudDTO Clouds { get; set; }
    public string Dt { get; set; }
    public SysDTO Sys { get; set; }
    public decimal TimeZone { get; set; }
    public int Id {get; set;}
    public string Name { get; set; }
    public string Cod {get; set;}
}