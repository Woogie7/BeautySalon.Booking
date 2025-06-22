using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeautySalon.Booking.Application.Service.Cache;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using MediatR;

namespace BeautySalon.Booking.Application.Features.Bookings.CancelBooking
{
    public record CancelBookingCommand(Guid Id, Guid ClientId): IRequest<Book>;
}
