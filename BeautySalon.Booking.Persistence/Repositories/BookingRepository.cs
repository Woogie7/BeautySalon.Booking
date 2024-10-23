using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Booking.Persistence.Context;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Persistence.Repositories
{
    internal class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _dbContext;

        public BookingRepository(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Book booking)
        {
            _dbContext.Books.Add(booking);
            _dbContext.SaveChanges();
        }


        public async Task<bool> IsBusyEmployeeAsync(Guid employeeId, Book booking)
        {
            return await _dbContext.Books
                .Where(b => b.EmployeeId == EmployeeId.Create(employeeId))
                .Where(b => b.Time.StartTime < booking.Time.EndTime && b.Time.EndTime < booking.Time.StartTime)
                .AnyAsync();
        }

        public async Task<bool> IsBusyClientAsync(Guid clientId, Book booking)
        {
            return await _dbContext.Books
                .Where(b => b.ClientId == ClientId.Create(clientId))
                .Where(b => b.Time.StartTime < booking.Time.EndTime && b.Time.EndTime < booking.Time.StartTime)
                .AnyAsync();
        }

        public async Task<bool> IsExistClientAsync(Guid clientId)
        {
            return await _dbContext.Clients
                .Where(e => e.Id == ClientId.Create(clientId))
                .AnyAsync();
        }

        public async Task<bool> IsExistEmployeeAsync(Guid employeeId)
        {
            return await _dbContext.Employees
                .Where(e => e.Id == EmployeeId.Create(employeeId))
                .AnyAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(Guid employeeId)
        {
           var asasda = await _dbContext.Employees.ToListAsync();

            return await _dbContext.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == EmployeeId.Create(employeeId));
        }
    }
}
