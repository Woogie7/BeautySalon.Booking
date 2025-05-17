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
using BeautySalon.Booking.Application.Models;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;

namespace BeautySalon.Booking.Persistence.Configurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeReadModel>
    {
        public void Configure(EntityTypeBuilder<EmployeeReadModel> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id); 

            builder.Property(e => e.Id)
                .ValueGeneratedNever(); 

            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
