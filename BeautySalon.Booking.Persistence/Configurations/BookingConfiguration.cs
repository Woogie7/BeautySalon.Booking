using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Booking.Domain.SeedWork;

namespace BeautySalon.Booking.Persistence.Configurations
{
    internal class BookingConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            ConfigureBookingTable(builder);
        }

        private void ConfigureBookingTable(EntityTypeBuilder<Book> builder)
        {
            
            builder.ToTable("Booking");

            builder.HasKey(b => b.Id);

            builder.Ignore(b => b.DomainEvents);


            builder.Property(b => b.Id)
                .ValueGeneratedNever()
                .HasConversion(
                id => id.Value,
                value => BookId.Create(value));

            builder.OwnsOne(b => b.Time);

            builder.Property(b => b.ClientId)
                .HasConversion(
                    id => id.Value,
                    value => ClientId.Create(value))
                .IsRequired();

            builder.Property(b => b.EmployeeId)
                .HasConversion(
                    id => id.Value,
                    value => EmployeeId.Create(value))
                .IsRequired();

            builder.Property(b => b.ServiceId)
                .HasConversion(
                    id => id.Value,
                    value => ServiceId.Create(value))
                .IsRequired();

            builder
            .Property(b => b.BookStatus)
            .HasConversion(
                v => v.Name,
                v => Enumeration.FromDisplayName<BookStatus>(v)
            )
            .IsRequired();

        }
    }

}
