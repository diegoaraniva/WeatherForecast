namespace Domain.Dto.WeatherManagement;

public class MainWeatherDTO
{
    public float Temp { get; set; }
    public float Feels_Like { get; set; }
    public float Temp_Min { get; set; }
    public float Temp_Max { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }
    public float Sea_Level { get; set; }
    public float Grnd_Level { get; set; }
    public float Temp_Kf { get; set; }
}