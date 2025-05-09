using BeautySalon.Booking.Application.Service.Cache;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;

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
    ) : IRequest<Result<Book>>, ICacheInvalidatingCommand<Book>
    {
       
        public CreateBookingCommand() : this(Guid.Empty, Guid.Empty, DateTime.MinValue, TimeSpan.Zero, Guid.Empty, 0m)
        {
        }

        public IEnumerable<string> GetCacheKeysToInvalidate()
        {
            yield return $"bookings:{ClientId}:::";
            yield return $"bookings::{EmployeeId}::";
            yield return $"bookings:::{StartTime}:";
            yield return $"bookings::::{StartTime-Duration}";
        }
    }
}
