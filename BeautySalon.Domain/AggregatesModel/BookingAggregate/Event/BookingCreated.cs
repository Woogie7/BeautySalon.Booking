using BeautySalon.Booking.Domain.SeedWork;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.Event
{
    public record class BookingCreated(Book Book) : IDomainEvent;
}
