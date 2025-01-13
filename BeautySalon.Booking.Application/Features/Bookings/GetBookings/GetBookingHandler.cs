using BeautySalon.Booking.Application.DTO.Booking;
using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Application.Service;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Application.Features.Bookings.GetBookings
{
    public class GetBookingHandler : IRequestHandler<GetBookingQuery, IEnumerable<BookDto>>
    {
        private readonly IBookService _bookService;

        public GetBookingHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetBookingsAsync(request.bookingFilter);
        }
    }
}
