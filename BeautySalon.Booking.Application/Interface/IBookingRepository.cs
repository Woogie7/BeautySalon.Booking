using BeautySalon.Booking.Application.DTO.Booking;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Application.Interface
{
    public interface IBookingRepository
    {
        Task<bool> IsBusyEmployeeAsync(Guid employeeId, Book booking);
        Task<bool> IsBusyClientAsync(Guid clientId, Book booking);
        Task<bool> IsExistClientAsync(Guid clientId);
        Task<Employee?> GetByIdEmployeeAsync(Guid employeeId);
        Task<Client?> GetByIdClientAsync(Guid clientId);
        Task CreateAsync(Book booking);
        Task<Book> GetByIdBookAsync(Guid bookId);

        Task<IEnumerable<Book>> GetBookingsAsync(BookingFilter bookingFilter);

        Task SaveChangesAsync();

        
    }
}
