using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Application.Features.Booking.CreateBooking
{
    public record CreateBookingCommand
    (
        Guid EmployeeId,
        Guid ClientId,
        DateTime StartTime,
        TimeSpan Duration,
        Guid ServiceId,
        decimal Discount
    ) : IRequest<Book>
    {
       
        public CreateBookingCommand() : this(Guid.Empty, Guid.Empty, DateTime.MinValue, TimeSpan.Zero, Guid.Empty, 0m)
        {
        }
    }
}
