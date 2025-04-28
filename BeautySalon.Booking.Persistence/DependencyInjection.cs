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
            var connectionString = $"Host={Environment.GetEnvironmentVariable("POSTGRES_HOST")};" +
                                   $"Port={Environment.GetEnvironmentVariable("POSTGRES_PORT")};" +
                                   $"Username={Environment.GetEnvironmentVariable("POSTGRES_USER")};" +
                                   $"Password={Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")};" +
                                   $"Database={Environment.GetEnvironmentVariable("POSTGRES_DB")};";
            
            service.AddDbContext<BookingDbContext>(o =>
            {
                o.UseNpgsql(connectionString);
            });
            service.AddScoped<IBookingRepository, BookingRepository>();

            return service;
        }
    }
}
