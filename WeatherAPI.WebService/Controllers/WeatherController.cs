using Application.Handlers.WeatherManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TechnicalTest.Controllers;

[ApiController]
public class WeatherController(ICurrentWeatherHandler currentWeatherHandler, 
    IForecastWeatherHandler forecastWeatherHandler) : Controller
{
    [HttpGet("current")]
    public IActionResult Current()
    {
        try
        {
            var response = currentWeatherHandler.GetCurrentWeather();
            return Ok(response.Result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error. Please try again later. " + ex.Message);
        }
    }
    
    [HttpGet("forecast")]
    public IActionResult Forecast()
    {
        try
        {
            var response = forecastWeatherHandler.GetForecast();
            return Ok(response.Result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error. Please try again later. " + ex.Message);
        }
    }
}