using BeautySalon.Booking.Application.Models;
using BeautySalon.Booking.Persistence.Context;
using BeautySalon.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Booking.Infrastructure.Rabbitmq.Consumers;

public class AvailabilityCreatedConsumer : IConsumer<AvailabilityCreatedEvent>
{
    private readonly BookingDbContext _context;
    private readonly ILogger<AvailabilityCreatedConsumer> _logger;

    public AvailabilityCreatedConsumer(BookingDbContext context, ILogger<AvailabilityCreatedConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AvailabilityCreatedEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Получено событие AvailabilityCreatedEvent: AvailabilityId = {AvailabilityId}, EmployeeId = {EmployeeId}, StartTime = {StartTime}, EndTime = {EndTime}", message.AvailabilityId, message.EmployeeId, message.StartTime, message.EndTime);

        try
        {
            var availability = new EmployeeAvailability
            {
                Id = message.AvailabilityId,
                EmployeeId = message.EmployeeId,
                StartTime = message.StartTime,
                EndTime = message.EndTime
            };

            _logger.LogInformation("Создание новой записи доступности сотрудника в базе данных...");

            _context.Availabilities.Add(availability);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Запись доступности успешно сохранена: AvailabilityId = {AvailabilityId}, EmployeeId = {EmployeeId}", availability.Id, availability.EmployeeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обработке события AvailabilityCreatedEvent: AvailabilityId = {AvailabilityId}, EmployeeId = {EmployeeId}", message.AvailabilityId, message.EmployeeId);
            throw;
        }
    }

}
