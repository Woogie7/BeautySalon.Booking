using AutoMapper;
using BeautySalon.Booking.Api;
using BeautySalon.Booking.Api.Middleware;
using BeautySalon.Booking.Application;
using BeautySalon.Booking.Application.DTO.Booking;
using BeautySalon.Booking.Application.Features.Booking.CreateBooking;
using BeautySalon.Booking.Application.Features.Bookings.ConfirmedBooking;
using BeautySalon.Booking.Application.Features.Bookings.GetBookings;
using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Infrastructure;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Booking.Infrastructure.Rabbitmq.Consumers;
using BeautySalon.Booking.Persistence;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddPersistance(builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()  
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddMassTransit(busConfing =>
{
    busConfing.SetKebabCaseEndpointNameFormatter();

    busConfing.AddConsumer<BookingConfirmedConsumer>();
    busConfing.AddConsumer<EmployeeEventsConsumer>();
    busConfing.AddConsumer<ServiceEventsConsumer>();
    busConfing.AddConsumer<ScheduleEmployeeEventsConsumer>();
    busConfing.AddConsumer<AvailabilityCreatedConsumer>();

    busConfing.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri("amqp://rabbitmq:5672"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("BookingChache");
    options.InstanceName = "Booking";
});

var app = builder.Build();

await app.MigrateDbAsync();

app.UseExceptionHandling();

app.MapPost("/bookings", async ([FromBody]CreateBookingRequest reqest, ISender _sender, IMapper _mapper) =>
{
    var command = _mapper.Map<CreateBookingCommand>(reqest);

    var createBookingResult = await _sender.Send(command);

    return Results.Ok(createBookingResult);
});

app.MapGet("/bookings", async ([AsParameters]BookingFilter bookingFilter, ISender _sender) =>
{
    var result = await _sender.Send(new GetBookingQuery(bookingFilter));

    if (result != null && result.Any())
        return Results.Ok(result);

    return Results.NotFound();
});

app.MapGet("/hello", async (ICacheService _cacheService) => 
{
    await _cacheService.SetAsync("123", "Привет мир", TimeSpan.FromSeconds(30));
    var sadfa = await _cacheService.GetAsync<string>("123");

    return Results.Ok(sadfa);
});

app.UseHttpsRedirection();
app.Run();

