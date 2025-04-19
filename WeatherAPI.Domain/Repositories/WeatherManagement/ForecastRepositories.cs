using Domain.Contexts;
using Domain.Models.WeatherManagement;
using Domain.Repositories.WeatherManagement.interfaces;

namespace Domain.Repositories.WeatherManagement;

public class ForecastRepositories : BaseRepositories<Forecast>, IForecastRepositories
{
    private readonly AppDBContext _context;
    
    public ForecastRepositories(AppDBContext context) : base(context) => _context = context;
    
    
}