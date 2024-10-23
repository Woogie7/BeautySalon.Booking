using AutoMapper;
using BeautySalon.Booking.Application;
using BeautySalon.Booking.Persistence;
using BeautySalon.Booking.Application.Features.Booking.CreateBooking;
using BeautySalon.Booking.Contracts;
using MediatR;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddPersistance(builder.Configuration);
builder.Services.AddStackExchangeRedisCache(o => o.Configuration = (builder.Configuration.GetConnectionString("BookingChache")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


app.MapPost("/client/{clientId}/booking", (CreateBookingRequest reqest, string clientId, ISender _sender, IMapper _mapper) =>
{
    var command = _mapper.Map<CreateBookingCommand>(reqest);
    
    var createBookingResult = _sender.Send(command);

    return "asd";
});


app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
