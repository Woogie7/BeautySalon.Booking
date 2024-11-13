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
    internal class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");

            builder.HasKey(c => c.Id);

            builder.Ignore(b => b.DomainEvents);

            builder.Property(c => c.Id)
                .ValueGeneratedNever()
                .HasConversion(
                id => id.Value,
                value => ClientId.Create(value));

            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
            builder.Property(c => c.SurnName).HasMaxLength(100);

            builder.Property(c => c.Email).HasMaxLength(255).IsRequired();
            builder.HasIndex(c => c.Email).IsUnique();

            builder.Property(c => c.Phone).HasMaxLength(11);
            builder.HasIndex(c => c.Phone).IsUnique();


        }
    }
}
