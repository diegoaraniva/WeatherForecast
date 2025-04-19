using Domain.Models.WeatherManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations.WeatherManagement;

public class ForecastConfigurations : IEntityTypeConfiguration<Forecast>
{
    public void Configure(EntityTypeBuilder<Forecast> builder)
    {
        builder.HasKey(f => f.Id);
        
        
        
        builder.Property(f => f.IsEnabled)
            .HasDefaultValue(true);
        builder.Property(f => f.CreationDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(f => f.LastUpdate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}