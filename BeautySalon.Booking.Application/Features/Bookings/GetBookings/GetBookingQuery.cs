using BeautySalon.Booking.Application.DTO.Booking;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Application.Features.Bookings.GetBookings
{
    public record GetBookingQuery(BookingFilter bookingFilter) : IRequest<IEnumerable<BookDto>>;
}
