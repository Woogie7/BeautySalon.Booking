using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Contracts;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Booking.Application.Features.Bookings.ConfirmedBooking;

public sealed class BookingConfirmedConsumer : IConsumer<BookingStatusChangedEvent>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ILogger<BookingConfirmedConsumer> _logger;
    private readonly ICacheService _cacheService;
    private readonly IEventBus _eventBus;
    public BookingConfirmedConsumer(IBookingRepository bookingRepository, ILogger<BookingConfirmedConsumer> logger, ICacheService cacheService, IEventBus eventBus)
    {
        _bookingRepository = bookingRepository;
        _logger = logger;
        _cacheService = cacheService;
        _eventBus = eventBus;
    }

    public async Task Consume(ConsumeContext<BookingStatusChangedEvent> context)
    {
        try
        {
            _logger.LogInformation("Received BookingStatusChangedEvent with ID: {Id}", context.Message.IdBooking);

            var booking = await _bookingRepository.GetByIdBookAsync(context.Message.IdBooking);
            if (booking == null)
            {
                _logger.LogWarning("Booking with ID: {Id} not found", context.Message.IdBooking);
                return;
            }

            var status = Domain.SeedWork.Enumeration.FromDisplayName<BookStatus>(context.Message.Status);

            if (status == BookStatus.Confirmed)
            {
                booking.ConfirmBooking();
                await _eventBus.SendMessageAsync(new BookingConfirmedEvent
                {
                    Id = booking.Id.Value,
                    StartTime = booking.Time.StartTime,
                    Duration = booking.Time.Duration,
                    ClientId = booking.ClientId.Value,
                    EmployeeId = booking.EmployeeId.Value,
                    ServiceId = booking.ServiceId.Value
                }, context.CancellationToken);
                
                _logger.LogInformation("Booking with ID: {Id} confirmed successfully", context.Message.IdBooking);
            }
            else if (status == BookStatus.Canceled)
            {
                booking.CancelBooking();
                
                await _eventBus.SendMessageAsync(new BookingCancelledEvent
                {
                    BookingId = booking.Id.Value,
                    EmployeeId = booking.EmployeeId.Value,
                    StartTime = booking.Time.StartTime,
                    Duration = booking.Time.Duration
                }, context.CancellationToken);
                
                _logger.LogInformation("Booking with ID: {Id} was canceled", context.Message.IdBooking);
            }
            else
            {
                _logger.LogWarning("Invalid status for Booking with ID: {Id}. Status: {Status}",
                    context.Message.IdBooking, context.Message.Status);
                return;
            }

            await _bookingRepository.SaveChangesAsync();

            var cacheKeys = GenerateCacheKeysForBooking(booking);
            foreach (var cacheKey in cacheKeys)
            {
                await _cacheService.RemoveAsync(cacheKey);
            }

            _logger.LogInformation("Cache invalidated for Booking ID: {Id}", context.Message.IdBooking);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing BookingConfirmedEvent with ID: {Id}", context.Message.IdBooking);
        }
    }

    private IEnumerable<string> GenerateCacheKeysForBooking(Book booking)
    {
        yield return $"bookings:{booking.ClientId.Value}:::";
        yield return $"bookings::{booking.EmployeeId.Value}::";
        yield return $"bookings:::{booking.Time.StartTime}:";
        yield return $"bookings::::{booking.Time.EndTime}";
    }
}

