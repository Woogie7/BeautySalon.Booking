using BeautySalon.Booking.Application.Behavior;
using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Application.Service;
using BeautySalon.Booking.Application.Service.Cache;
using BeautySalon.Booking.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BeautySalon.Booking.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection service)
        {
            service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            service.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            service.AddTransient<ICacheService, CacheService>();
            service.AddTransient<IBookService, BookService>();

            service.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationPipelineBehavior<,>));
            
            service.AddValidatorsFromAssemblyContaining<CreateBookingRequestValidator>();
            service.AddFluentValidationAutoValidation();

            
            return service;
        }
    }
}
