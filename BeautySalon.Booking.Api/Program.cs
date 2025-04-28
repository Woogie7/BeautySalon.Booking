using AutoMapper;
using BeautySalon.Booking.Persistence;
using MediatR;
using BeautySalon.Booking.Api;
using MassTransit;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Booking.Infrastructure;
using BeautySalon.Booking.Application;
using BeautySalon.Booking.Application.Features.Booking.CreateBooking;
using BeautySalon.Booking.Application.DTO.Booking;
using BeautySalon.Booking.Application.Features.Bookings.Confirmed;
using BeautySalon.Booking.Application.Features.Bookings.GetBookings;
using Microsoft.AspNetCore.Mvc;
using BeautySalon.Booking.Application.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddPersistance(builder.Configuration);

builder.Services.AddMassTransit(busConfing =>
{
    busConfing.SetKebabCaseEndpointNameFormatter();

    busConfing.AddConsumer<BookingConfirmedConsumer>();

    busConfing.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri("amqp://beautysalon.booking.rabbitmq:5672"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        configurator.ConfigureEndpoints(context);
    });

});

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddStackExchangeRedisCache(o => o.Configuration = (builder.Configuration.GetConnectionString("BookingChache")));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateDbAsync();
}

app.UseHttpsRedirection();

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

app.MapPost("/confirmed", async (ConfirmBooked reqest, ISender _sender) =>
{
    await _sender.Send(reqest);

    return Results.Ok();
});

app.MapGet("/hello", async (ICacheService _cacheService) => 
{
    await _cacheService.SetAsync("123", "Привет йцуйц", TimeSpan.FromSeconds(30));
    var sadfa = await _cacheService.GetAsync<string>("123");

    return Results.Ok(sadfa);
});

app.Run();

