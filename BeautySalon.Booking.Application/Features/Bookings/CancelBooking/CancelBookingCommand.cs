using BeautySalon.Booking.Application.Service.Cache;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Application.Features.Bookings.CancelBooking
{
    public record CancelBookingCommand(Guid Id, string Reason): IRequest<Book>;
}
