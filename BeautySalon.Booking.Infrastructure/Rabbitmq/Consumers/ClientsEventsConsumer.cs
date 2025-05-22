using BeautySalon.Booking.Application.Models;
using BeautySalon.Booking.Persistence.Context;
using BeautySalon.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Booking.Infrastructure.Rabbitmq.Consumers;

public class ClientEventsConsumer : IConsumer<ClientCreatedEvent>
{
    private readonly BookingDbContext _context;
    private readonly ILogger<ClientEventsConsumer> _logger;

    public ClientEventsConsumer(BookingDbContext context, ILogger<ClientEventsConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ClientCreatedEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation("Получено событие создания клиента: {Id}", message.UserId);

        if (await _context.Clients.FindAsync(message.UserId) != null)
        {
            _logger.LogWarning("Клиент с Id {Id} уже существует", message.UserId);
            return;
        }

        var client = new ClientReadModel()
        {
            Id = message.UserId,
            Name = message.FirstName,
            SurnName = message.LastName,
            Email = message.Email,
            Phone = message.Phone,
            BerthDay = new DateTime(2005, 4, 29).ToUniversalTime()
        };

        _context.Clients.Add(client);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Клиент {Id} и его расписание успешно сохранены", message.UserId);
    }
}