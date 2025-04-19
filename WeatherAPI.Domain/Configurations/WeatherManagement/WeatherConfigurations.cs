using Domain.Models.WeatherManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations.WeatherManagement;

public class WeatherConfigurations : IEntityTypeConfiguration<Weather>
{
    public void Configure(EntityTypeBuilder<Weather> builder)
    {
        builder.HasKey(w => w.Id);
        
        
        
        builder.Property(w => w.IsEnabled)
            .HasDefaultValue(true);
        builder.Property(w => w.CreationDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(w => w.LastUpdate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}