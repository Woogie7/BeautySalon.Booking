using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BeautySalon.Booking.Persistence.Context;

public class BookingDbContextFactory : IDesignTimeDbContextFactory<BookingDbContext>
{
    public BookingDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookingDbContext>();
        optionsBuilder.UseNpgsql("Host=booking-service-postgres;Port=5432;Database=BeautySalonBookingDb;Username=postgres;Password=1234");

        return new BookingDbContext(optionsBuilder.Options);
    }
}