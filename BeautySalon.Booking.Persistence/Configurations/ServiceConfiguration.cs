using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
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
    internal class ServiceConfiguration : IEntityTypeConfiguration<ServiceReadModel>
    {
        public void Configure(EntityTypeBuilder<ServiceReadModel> builder)
        {
            builder.ToTable("Services");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .ValueGeneratedNever();

            builder.Property(s => s.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(s => s.Description)
                .HasMaxLength(500);

            builder.Property(s => s.Duration)
                .IsRequired();

            builder.Property(s => s.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
