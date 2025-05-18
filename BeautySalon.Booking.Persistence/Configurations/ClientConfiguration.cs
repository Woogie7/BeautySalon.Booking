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

namespace BeautySalon.Booking.Persistence.Configurations
{
    internal class ClientConfiguration : IEntityTypeConfiguration<ClientReadModel>
    {
        public void Configure(EntityTypeBuilder<ClientReadModel> builder)
        {
            builder.ToTable("Clients");

            builder.HasKey(e => e.Id); 

            builder.Property(e => e.Id)
                .ValueGeneratedNever(); 

            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();
            
            builder.Property(e => e.SurnName)
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
