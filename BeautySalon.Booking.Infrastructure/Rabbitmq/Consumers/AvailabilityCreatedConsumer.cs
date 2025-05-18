using BeautySalon.Booking.Application.Models;
using BeautySalon.Booking.Persistence.Context;
using BeautySalon.Contracts;
using MassTransit;

namespace BeautySalon.Booking.Infrastructure.Rabbitmq.Consumers;

public class AvailabilityCreatedConsumer : IConsumer<AvailabilityCreatedEvent>
{
    private readonly BookingDbContext _context;

    public AvailabilityCreatedConsumer(BookingDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<AvailabilityCreatedEvent> context)
    {
        var message = context.Message;

        var availability = new EmployeeAvailability
        {
            Id = message.AvailabilityId,
            EmployeeId = message.EmployeeId,
            StartTime = message.StartTime,
            EndTime = message.EndTime
        };

        _context.Availabilities.Add(availability);
        await _context.SaveChangesAsync();
    }
}
