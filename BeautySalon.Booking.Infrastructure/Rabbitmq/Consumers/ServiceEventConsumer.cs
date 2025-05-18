using BeautySalon.Booking.Application.Models;
using BeautySalon.Booking.Persistence.Context;
using BeautySalon.Contracts.Service;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Booking.Infrastructure.Rabbitmq.Consumers;

public class ServiceEventsConsumer :
    IConsumer<ServiceCreatedEvent>,
    IConsumer<ServiceUpdatedEvent>,
    IConsumer<ServiceDeletedEvent>
{
    private readonly BookingDbContext _context;
    private readonly ILogger<ServiceEventsConsumer> _logger;

    public ServiceEventsConsumer(BookingDbContext context, ILogger<ServiceEventsConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ServiceCreatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received ServiceCreatedEvent for ServiceId: {ServiceId}", message.Id);

        if (await _context.Services.FindAsync(message.Id) != null)
        {
            _logger.LogWarning("Service with Id {ServiceId} already exists. Skipping creation.", message.Id);
            return;
        }

        var service = new ServiceReadModel
        {
            Id = message.Id,
            Name = message.Name,
            Description = message.Description,
            Duration = message.Duration,
            Price = message.Price
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Service with Id {ServiceId} created successfully.", message.Id);
    }

    public async Task Consume(ConsumeContext<ServiceUpdatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received ServiceUpdatedEvent for ServiceId: {ServiceId}", message.Id);

        var service = await _context.Services.FindAsync(message.Id);
        if (service == null)
        {
            _logger.LogWarning("Service with Id {ServiceId} not found. Skipping update.", message.Id);
            return;
        }

        service.Name = message.Name;
        service.Description = message.Description;
        service.Duration = message.Duration;
        service.Price = message.Price;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Service with Id {ServiceId} updated successfully.", message.Id);
    }

    public async Task Consume(ConsumeContext<ServiceDeletedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received ServiceDeletedEvent for ServiceId: {ServiceId}", message.Id);

        var service = await _context.Services.FindAsync(message.Id);
        if (service == null)
        {
            _logger.LogWarning("Service with Id {ServiceId} not found. Skipping deletion.", message.Id);
        }
        else
        {
            _context.Services.Remove(service);
        }

        var affectedEmployees = await _context.Employees
            .Where(e => e.ServiceIds.Contains(message.Id))
            .ToListAsync();

        foreach (var employee in affectedEmployees)
        {
            employee.ServiceIds.Remove(message.Id);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Service with Id {ServiceId} deleted and unlinked from employees.", message.Id);
    }
}
