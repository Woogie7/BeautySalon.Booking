using BeautySalon.Booking.Application.Service.Cache;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using MediatR;

namespace BeautySalon.Booking.Application.Features.Bookings.CreateBooking
{
    public record CreateBookingCommand
    (
        Guid EmployeeId,
        Guid ClientId,
        DateTime StartTime,
        TimeSpan Duration,
        Guid ServiceId
    ) : IRequest<Book>, ICacheInvalidatingCommand<Book>
    {
       
        public CreateBookingCommand() : this(Guid.Empty, Guid.Empty, DateTime.MinValue, TimeSpan.Zero, Guid.Empty)
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
