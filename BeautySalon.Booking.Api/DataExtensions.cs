using BeautySalon.Booking.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Booking.Api
{
    public static class DataExtensions
    {
        public static async Task MigrateDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var DbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
            await DbContext.Database.MigrateAsync();
        }
    }
}
