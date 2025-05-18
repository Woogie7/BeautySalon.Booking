using BeautySalon.Booking.Application.Models;
using BeautySalon.Booking.Persistence.Context;
using BeautySalon.Contracts.Schedule;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Booking.Infrastructure.Rabbitmq.Consumers;


public class ScheduleEmployeeEventsConsumer :
    IConsumer<ScheduleAddedEmployeeEvent>,
    IConsumer<ScheduleUpdatedEmployeeEvent>,
    IConsumer<ScheduleRemovedEmployeeEvent>
{
    private readonly BookingDbContext _context;
    private readonly ILogger<ScheduleEmployeeEventsConsumer> _logger;

    public ScheduleEmployeeEventsConsumer(BookingDbContext context, ILogger<ScheduleEmployeeEventsConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ScheduleAddedEmployeeEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received EmployeeScheduleAddedEvent for EmployeeId: {EmployeeId}, ScheduleId: {ScheduleId}", message.EmployeeId, message.ScheduleId);

        var employee = await _context.Employees
            .Include(e => e.Schedules)
            .FirstOrDefaultAsync(e => e.Id == message.EmployeeId);

        if (employee == null)
        {
            _logger.LogWarning("Employee with Id {EmployeeId} not found.", message.EmployeeId);
            return;
        }

        var existing = employee.Schedules.FirstOrDefault(s => s.Id == message.ScheduleId);
        if (existing != null)
        {
            _logger.LogWarning("Schedule with Id {ScheduleId} already exists for Employee {EmployeeId}.", message.ScheduleId, message.EmployeeId);
            return;
        }

        var schedule = new ScheduleReadModel
        {
            Id = message.ScheduleId,
            EmployeeId = message.EmployeeId,
            DayOfWeek = message.DayOfWeek == 7 ? DayOfWeek.Sunday : (DayOfWeek)message.DayOfWeek,
            StartTime = message.StartTime,
            EndTime = message.EndTime
        };

        employee.Schedules.Add(schedule);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Schedule {ScheduleId} added for Employee {EmployeeId}.", message.ScheduleId, message.EmployeeId);
    }

    public async Task Consume(ConsumeContext<ScheduleUpdatedEmployeeEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received EmployeeScheduleUpdatedEvent for ScheduleId: {ScheduleId}", message.ScheduleId);

        var schedule = await _context.Schedules
            .FirstOrDefaultAsync(s => s.Id == message.ScheduleId && s.EmployeeId == message.EmployeeId);

        if (schedule == null)
        {
            _logger.LogWarning("Schedule with Id {ScheduleId} not found for Employee {EmployeeId}.", message.ScheduleId, message.EmployeeId);
            return;
        }

        schedule.DayOfWeek = message.DayOfWeek == 7 ? DayOfWeek.Sunday : (DayOfWeek)message.DayOfWeek;
        schedule.StartTime = message.StartTime;
        schedule.EndTime = message.EndTime;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Schedule {ScheduleId} updated successfully.", message.ScheduleId);
    }

    public async Task Consume(ConsumeContext<ScheduleRemovedEmployeeEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received EmployeeScheduleRemovedEvent for ScheduleId: {ScheduleId}", message.ScheduleId);

        var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == message.ScheduleId && s.EmployeeId == message.EmployeeId);

        if (schedule == null)
        {
            _logger.LogWarning("Schedule with Id {ScheduleId} not found for deletion.");
            return;
        }

        _context.Schedules.Remove(schedule);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Schedule {ScheduleId} removed successfully.", message.ScheduleId);
    }
}