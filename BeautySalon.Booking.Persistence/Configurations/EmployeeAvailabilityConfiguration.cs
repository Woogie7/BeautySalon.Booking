using BeautySalon.Booking.Application.Models;

namespace BeautySalon.Booking.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EmployeeAvailabilityConfiguration : IEntityTypeConfiguration<EmployeeAvailability>
{
    public void Configure(EntityTypeBuilder<EmployeeAvailability> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.StartTime)
            .IsRequired();

        builder.Property(e => e.EndTime)
            .IsRequired();

        builder.HasOne(e => e.Employee)
            .WithMany(emp => emp.Availabilities) // Предполагается, что в Employee есть коллекция Availabilities
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
