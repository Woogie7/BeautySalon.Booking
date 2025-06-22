using BeautySalon.Booking.Application.Models;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Booking.Domain.SeedWork;
using BeautySalon.Booking.Persistence.Repositories;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Booking.Persistence.Context
{
    public class BookingDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<ClientReadModel> Clients { get; set; } = null!;
        public DbSet<ServiceReadModel> Services { get; set; } = null!;
        public DbSet<EmployeeReadModel> Employees { get; set; } = null!;
        public DbSet<ScheduleReadModel> Schedules { get; set; } = null!;
        public DbSet<EmployeeAvailability> Availabilities { get; set; } = null!;
        
        public DbSet<PendingQueueMessage> PendingQueueMessages { get; set; } = null!;

        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            var bookStatus = Enumeration.GetAll<BookStatus>();

            modelBuilder.Entity<BookStatus>().HasData(bookStatus);
        }
    }
}
