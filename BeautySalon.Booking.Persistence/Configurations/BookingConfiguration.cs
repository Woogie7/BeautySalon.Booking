using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;
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

            builder.HasOne<Client>()
                .WithMany()
                .HasForeignKey(b => b.ClientId)
                .IsRequired();

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(b => b.EmployeeId)
                .IsRequired();

            builder.HasOne<Service>()
                .WithMany()
                .HasForeignKey(b => b.ServiceId)
                .IsRequired();

            builder
            .Property(b => b.BookStatus)
            .HasConversion(
                v => v.Name, // Преобразование из BookingStatus в строку
                v => BookStatus.FromDisplayName<BookStatus>(v) // Преобразование из строки в BookingStatus
            )
            .IsRequired();

        }
    }
}
