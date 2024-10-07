using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Persistence.Context;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
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
    }
}
