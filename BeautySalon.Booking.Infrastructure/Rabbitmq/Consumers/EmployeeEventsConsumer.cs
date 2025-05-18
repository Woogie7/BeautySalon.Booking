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

        _logger.LogInformation("Получено событие создания сотрудника: {Id}", message.Id);

        // Проверка на существование
        if (await _context.Employees.FindAsync(message.Id) != null)
        {
            _logger.LogWarning("Сотрудник с Id {Id} уже существует", message.Id);
            return;
        }

        var employee = new EmployeeReadModel
        {
            Id = message.Id,
            Name = message.Name,
            Email = message.Email,
            Phone = message.Phone,
            ServiceIds = message.ServiceIds
        };

        var schedules = message.Schedule.Select(s => new ScheduleReadModel
        {
            EmployeeId = message.Id,
            DayOfWeek = s.DayOfWeek,
            StartTime = s.StartTime,
            EndTime = s.EndTime
        }).ToList();

        _context.Employees.Add(employee);
        _context.Schedules.AddRange(schedules);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Сотрудник {Id} и его расписание успешно сохранены", message.Id);
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
        
        var newServiceIds = message.ServiceIds?.Distinct().ToList() ?? new List<Guid>();

        employee.ServiceIds = employee.ServiceIds
            .Where(id => newServiceIds.Contains(id))
            .ToList();
        
        var toAdd = newServiceIds.Except(employee.ServiceIds).ToList();
        employee.ServiceIds.AddRange(toAdd);

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
