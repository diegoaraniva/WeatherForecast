namespace Domain.Dto.WeatherManagement.ForecastWeather;

public class SingleForecastDTO
{
    public int Dt { get; set; }
    public MainWeatherDTO Main { get; set; }
    public List<WeatherDTO> Weather { get; set; }
    public CloudDTO Clouds { get; set; }
    public WindDTO Wind { get; set; }
    public int Visibility { get; set; }
    public decimal Pop { get; set; }
    public SysDTO Sys { get; set; }
    public DateTime Dt_Txt { get; set; }
}