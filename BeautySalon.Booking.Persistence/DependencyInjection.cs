using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Persistence.Context;
using BeautySalon.Booking.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection service, IConfiguration confing)
        {
            var сonnection = confing.GetConnectionString("BookingDatabase");
            service.AddDbContext<BookingDbContext>(o =>
            {
                o.UseNpgsql(сonnection);
            });
            service.AddScoped<IBookingRepository, BookingRepository>();

            return service;
        }
    }
}
