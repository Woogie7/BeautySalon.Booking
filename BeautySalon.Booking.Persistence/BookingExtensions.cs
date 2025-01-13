using BeautySalon.Booking.Application.DTO.Booking;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Persistence
{
    public static class BookingExtensions
    {
        public static IQueryable<Book> Filter(this IQueryable<Book> query, BookingFilter bookingFilter)
        {
            if(bookingFilter.ClientId.HasValue)
                query = query.Where(b => b.ClientId == ClientId.Create(bookingFilter.ClientId.Value));

            if (bookingFilter.EmployeeId.HasValue)
                query = query.Where(b => b.EmployeeId == EmployeeId.Create(bookingFilter.EmployeeId.Value));

            if(bookingFilter.StartDate.HasValue)
                query = query.Where(b => b.Time.StartTime >= bookingFilter.StartDate.Value);

            if (bookingFilter.EndDate.HasValue)
                query = query.Where(b => b.Time.EndTime <= bookingFilter.EndDate.Value);

            return query;
        }
    }
}
