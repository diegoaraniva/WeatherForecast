using System.Reflection;
using Domain.Models.WeatherManagement;
using Microsoft.EntityFrameworkCore;

namespace Domain.Contexts;

public class AppDBContext : DbContext
{
    public DbSet<Weather> Weathers { get; set; }
    public DbSet<Forecast> Forecasts { get; set; }
    
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) 
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}