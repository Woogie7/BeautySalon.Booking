using BeautySalon.Booking.Application.Models;
using BeautySalon.Booking.Persistence.Context;
using BeautySalon.Contracts.Employees;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Booking.Infrastructure.Rabbitmq.Consumers;

public class EmployeeEventsConsumer :
    IConsumer<EmployeeCreatedEvent>,
    IConsumer<EmployeeUpdatedEvent>,
    IConsumer<EmployeeDeletedEvent>
{
    private readonly BookingDbContext _context;
    private readonly ILogger<EmployeeEventsConsumer> _logger;

    public EmployeeEventsConsumer(BookingDbContext context, ILogger<EmployeeEventsConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EmployeeCreatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received EmployeeCreatedEvent for EmployeeId: {EmployeeId}", message.Id);

        if (await _context.Employees.FindAsync(message.Id) != null)
        {
            _logger.LogWarning("Employee with Id {EmployeeId} already exists. Skipping creation.", message.Id);
            return;
        }

        var employee = new EmployeeReadModel
        {
            Id = message.Id,
            Name = message.Name,
            Email = message.Email,
            Phone = message.Phone
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Employee with Id {EmployeeId} created successfully.", message.Id);
    }

    public async Task Consume(ConsumeContext<EmployeeUpdatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received EmployeeUpdatedEvent for EmployeeId: {EmployeeId}", message.Id);

        var employee = await _context.Employees.FindAsync(message.Id);
        if (employee == null)
        {
            _logger.LogWarning("Employee with Id {EmployeeId} not found. Skipping update.", message.Id);
            return;
        }

        employee.Name = message.Name;
        employee.Email = message.Email;
        employee.Phone = message.Phone;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Employee with Id {EmployeeId} updated successfully.", message.Id);
    }

    public async Task Consume(ConsumeContext<EmployeeDeletedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received EmployeeDeletedEvent for EmployeeId: {EmployeeId}", message.Id);

        var employee = await _context.Employees.FindAsync(message.Id);
        if (employee == null)
        {
            _logger.LogWarning("Employee with Id {EmployeeId} not found. Skipping deletion.", message.Id);
            return;
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Employee with Id {EmployeeId} deleted successfully.", message.Id);
    }
}
