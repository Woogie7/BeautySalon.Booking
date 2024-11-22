using AutoMapper;
using BeautySalon.Booking.Application;
using BeautySalon.Booking.Persistence;
using BeautySalon.Booking.Application.Features.Booking.CreateBooking;
using MediatR;
using BeautySalon.Booking.Api;
using BeautySalon.Booking.Infrastructure;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Booking.Application.DTO;
using MassTransit;
using BeautySalon.Booking.Application.Features.Bookings.CreateBooking;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddPersistance(builder.Configuration);

builder.Services.AddMassTransit(busConfing =>
{
    busConfing.SetKebabCaseEndpointNameFormatter();

    busConfing.AddConsumer<BookingConfirmedConsumer>();

    busConfing.UsingRabbitMq((context, configurator) =>
    {
        MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();

        configurator.Host(new Uri("localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        configurator.ConfigureEndpoints(context);
    });

});

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));
//builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().ToString());

builder.Services.AddStackExchangeRedisCache(o => o.Configuration = (builder.Configuration.GetConnectionString("BookingChache")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.MigrateDbAsync();
}

app.UseHttpsRedirection();

app.MapPost("/booking", async (CreateBookingRequest reqest, ISender _sender, IMapper _mapper) =>
{
    var command = _mapper.Map<CreateBookingCommand>(reqest);
    
    var createBookingResult = await _sender.Send(command);

    return Results.Ok(createBookingResult);
});

app.Run();

