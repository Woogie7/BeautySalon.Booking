using AutoMapper;
using BeautySalon.Booking.Application;
using BeautySalon.Booking.Persistence;
using BeautySalon.Booking.Application.Features.Booking.CreateBooking;
using BeautySalon.Booking.Contracts;
using MediatR;
using BeautySalon.Booking.Api;
using BeautySalon.Booking.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddPersistance(builder.Configuration);
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



app.MapPost("/client/{clientId}/booking", async (CreateBookingRequest reqest, string clientId, ISender _sender, IMapper _mapper) =>
{
    var command = _mapper.Map<CreateBookingCommand>(reqest);
    
    var createBookingResult = await _sender.Send(command);

    return Results.Ok(createBookingResult);
});

app.Run();

