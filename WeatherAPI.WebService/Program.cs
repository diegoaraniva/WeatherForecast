using Application.Handlers.WeatherManagement;
using Application.Handlers.WeatherManagement.Interfaces;
using Azure.Identity;
using Domain;
using Domain.Contexts;
using Domain.Repositories.WeatherManagement;
using Domain.Repositories.WeatherManagement.interfaces;
using Domain.Repositories.WeatherManagement.Interfaces;
using Domain.Secrets;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(Environment.GetEnvironmentVariable("FrontEndOrigin") ?? string.Empty)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

var connectionString = (builder.Environment.IsDevelopment())
    ? Environment.GetEnvironmentVariable("ConnStringDev") ?? string.Empty
    : Environment.GetEnvironmentVariable("ConnString") ?? string.Empty;

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlite(connectionString)
        .EnableSensitiveDataLogging()
        .LogTo(msg => Log.Information(msg)));

builder.Services.AddSingleton<CircuitBreakerPolicyProvider>();
        
builder.Services.AddScoped<ICurrentWeatherService, CurrentWeatherService>();
builder.Services.AddScoped<IForecastService, ForecastService>();

builder.Services.AddScoped<IWeatherRepositories, WeatherRepositories>();
builder.Services.AddScoped<IForecastRepositories, ForecastRepositories>();

builder.Services.AddScoped<ICurrentWeatherHandler, CurrentWeatherHandler>();
builder.Services.AddScoped<IForecastWeatherHandler, ForecastWeatherHandler>();

if (!builder.Environment.IsDevelopment())
    builder.WebHost.UseUrls("http://0.0.0.0:80");

builder.Host.UseSerilog();

builder.Services.Configure<OpenApi>(builder.Configuration);

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
}

app.UseCors("AllowFrontend");

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "/{controller}/{action}/{id?}");

app.UseAuthorization();

app.Run();
