using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Persistence.Configuration
{
    internal class BookingConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.BookingAggregate.Book>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.BookingAggregate.Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.OwnsOne(b => b.Time, time =>
            {
                time.WithOwner();
            });
        }
    }
}
