using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Booking.Persistence.Context;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Booking.Persistence.Repositories
{
    internal class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _dbContext;

        public BookingRepository(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsBusyEmployeeAsync(Guid employeeId, Book booking)
        {
            return await _dbContext.Books
                .Where(b => b.EmployeeId == EmployeeId.Create(employeeId))
                .Where(b => b.Time.StartTime < booking.Time.EndTime && b.Time.EndTime > booking.Time.StartTime)
                .AnyAsync();
        }

        public async Task<bool> IsBusyClientAsync(Guid clientId, Book booking)
        {
            return await _dbContext.Books
                .Where(b => b.ClientId == ClientId.Create(clientId))
                .Where(b => b.Time.StartTime < booking.Time.EndTime && b.Time.EndTime > booking.Time.StartTime)
                .AnyAsync();
        }

        public async Task<bool> IsExistClientAsync(Guid clientId)
        {
            return await _dbContext.Clients
                .Where(e => e.Id == ClientId.Create(clientId))
                .AnyAsync();
        }

        public async Task<Employee?> GetByIdEmployeeAsync(Guid employeeId)
        {

            var employee = await _dbContext.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == EmployeeId.Create(employeeId));
            return employee;
        }

        public async Task<Client?> GetByIdClientAsync(Guid clientId)
        {
            var client = await _dbContext.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == ClientId.Create(clientId));

            return client;
        }

        public async Task CreateAsync(Book booking)
        {
            await _dbContext.Books.AddAsync(booking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Book> GetByIdBookAsync(Guid bookId)
        {
            return await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == BookId.Create(bookId));
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
