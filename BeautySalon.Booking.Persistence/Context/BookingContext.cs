using BeautySalon.Booking.Persistence.Configuration;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Persistence.Context
{
    public class BookingContext : DbContext
    {
        public DbSet<Domain.AggregatesModel.BookingAggregate.Book> Books { get; set; }

        public BookingContext(DbContextOptions<BookingContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookingConfiguration());
        }
    }
}
