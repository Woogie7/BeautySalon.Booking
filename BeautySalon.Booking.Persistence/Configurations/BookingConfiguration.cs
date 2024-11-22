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

            builder
                .Property<int>("_bookStatusId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("BookStatusId")
                .IsRequired();

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

            builder.HasOne(o => o.BookStatus)
                .WithMany()
                .HasForeignKey("_bookStatusId");
        }
    }
}
