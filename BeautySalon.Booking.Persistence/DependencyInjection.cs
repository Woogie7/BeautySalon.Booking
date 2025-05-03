using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Persistence.Context;
using BeautySalon.Booking.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeautySalon.Booking.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection service, IConfiguration confing)
        {
            var connectionString = confing.GetConnectionString("Host=booking-service-api-postgres;Port=5432;Database=BeautySalonBookingDb;Username=postgres;Password=1234");
            
            service.AddDbContext<BookingDbContext>(o =>
            {
                o.UseNpgsql(connectionString);
            });
            service.AddScoped<IBookingRepository, BookingRepository>();

            return service;
        }
    }
}
