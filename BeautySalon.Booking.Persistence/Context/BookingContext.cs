using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Booking.Persistence.Configurations;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Persistence.Context
{
    public class BookingDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;

        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
        }
    }
}
