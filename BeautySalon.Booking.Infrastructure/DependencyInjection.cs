using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Application.Interface.DB;
using BeautySalon.Booking.Application.Service;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using Microsoft.Extensions.DependencyInjection;

namespace BeautySalon.Booking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service)
        {
            service.AddTransient<IEventBus, EventBus>();
            service.AddTransient<IEmployeeReedService, EmployeeReedService>();
            service.AddTransient<IClientReadService, ClientReadService>();
            return service;
        }
    }
}
