using Application.Handlers.WeatherManagement;
using Application.Handlers.WeatherManagement.Interfaces;
using Domain;
using Domain.Contexts;
using Domain.Repositories.WeatherManagement;
using Domain.Repositories.WeatherManagement.interfaces;
using Domain.Repositories.WeatherManagement.Interfaces;
using Domain.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

var connectionString = (builder.Environment.IsDevelopment())
    ? builder.Configuration.GetConnectionString("DefaultDevConnection")
    : builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddSingleton<CircuitBreakerPolicyProvider>();
        
builder.Services.AddScoped<ICurrentWeatherService, CurrentWeatherService>();
builder.Services.AddScoped<IForecastService, ForecastService>();

builder.Services.AddScoped<IWeatherRepositories, WeatherRepositories>();
builder.Services.AddScoped<IForecastRepositories, ForecastRepositories>();

builder.Services.AddScoped<ICurrentWeatherHandler, CurrentWeatherHandler>();
builder.Services.AddScoped<IForecastWeatherHandler, ForecastWeatherHandler>();

builder.WebHost.UseUrls("http://0.0.0.0:80");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowFrontend");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "/{controller}/{action}/{id?}");

app.Run();