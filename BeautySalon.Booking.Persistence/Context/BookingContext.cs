using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using Microsoft.EntityFrameworkCore;

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
