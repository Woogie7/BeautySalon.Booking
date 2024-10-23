using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Persistence.Configurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(c => c.Id);

            builder.Ignore(b => b.DomainEvents);

            builder.Property(c => c.Id)
                .ValueGeneratedNever()
                .HasConversion(
                id => id.Value,
                value => EmployeeId.Create(value));

            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        }
    }
}
