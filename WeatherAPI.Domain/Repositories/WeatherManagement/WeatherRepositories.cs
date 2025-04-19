using System.Linq.Expressions;
using Domain.Contexts;
using Domain.Models.WeatherManagement;
using Domain.Repositories.WeatherManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.WeatherManagement;

public class WeatherRepositories : BaseRepositories<Weather>, IWeatherRepositories
{
    private readonly AppDBContext _context;
    
    public WeatherRepositories(AppDBContext context) : base(context) => _context = context;
    
    public async Task<Weather?> GetLastAsync(Expression<Func<Weather, object>> orderBy)
    {
        return await _context.Set<Weather>()
            .OrderByDescending(orderBy)
            .FirstOrDefaultAsync();
    }
}