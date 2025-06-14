using System.Security.Claims;
using System.Text;
using AutoMapper;
using BeautySalon.Booking.Api;
using BeautySalon.Booking.Api.Middleware;
using BeautySalon.Booking.Application;
using BeautySalon.Booking.Application.DTO.Booking;
using BeautySalon.Booking.Application.Features.Bookings.CancelBooking;
using BeautySalon.Booking.Application.Features.Bookings.CreateBooking;
using BeautySalon.Booking.Application.Features.Bookings.GetBookings;
using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Infrastructure;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Booking.Infrastructure.Rabbitmq.Consumers;
using BeautySalon.Booking.Persistence;
using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>()!;

Log.Logger.Information("JwtOptions in Employees Service: SecretKey = {SecretKey}, Issuer = {Issuer}, Audience = {Audience}",
    jwtOptions.SecretKey, jwtOptions.Issuer, jwtOptions.Audience);

var key = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
        
    options.AddPolicy("EmployeeOnly", policy =>
        policy.RequireRole("Employee"));
        
    options.AddPolicy("ClientOnly", policy =>
        policy.RequireRole("Client"));
});

builder.Services.AddMassTransit(busConfing =>
{
    busConfing.SetKebabCaseEndpointNameFormatter();

    busConfing.AddConsumer<BookingConfirmedConsumer>();
    busConfing.AddConsumer<EmployeeEventsConsumer>();
    busConfing.AddConsumer<ServiceEventsConsumer>();
    busConfing.AddConsumer<ScheduleEmployeeEventsConsumer>();
    busConfing.AddConsumer<AvailabilityCreatedConsumer>();
    busConfing.AddConsumer<ClientEventsConsumer>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/bookings", async (
    HttpContext context,
    [FromBody] CreateBookingRequest request,
    ISender sender,
    IMapper mapper,
    IValidator<CreateBookingRequest> validator) =>
{
    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrWhiteSpace(userId))
        return Results.Unauthorized();

    var validationResult = await validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
    }

    var clientId = Guid.Parse(userId);

    var command = new CreateBookingCommand(
        request.EmployeeId,
        clientId,
        request.StartTime,
        request.Duration,
        request.ServiceId
    );

    var createBookingResult = await sender.Send(command);
    return Results.Ok(createBookingResult);
}).RequireAuthorization("ClientOnly");


app.MapGet("/bookings", async ([AsParameters]BookingFilter bookingFilter, ISender _sender) =>
{
    var result = await _sender.Send(new GetBookingQuery(bookingFilter));

    if (result != null && result.Any())
        return Results.Ok(result);

    return Results.NotFound();
}).RequireAuthorization();

app.MapDelete("/bookings", async (
    HttpContext context,
    Guid idBook,
    ISender _sender) =>
{
    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


    if (string.IsNullOrWhiteSpace(userId))
        return Results.Unauthorized();

    var clientId = Guid.Parse(userId);

    var command = new CancelBookingCommand(idBook, clientId);

    await _sender.Send(command);
    return Results.NoContent();
}).RequireAuthorization("ClientOnly");

app.UseHttpsRedirection();
app.Run();

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; }
}

