using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Application.Service.Cache;
using BeautySalon.Booking.Contracts;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Booking.Application.Features.Bookings.Confirmed;

public sealed class BookingConfirmedConsumer : IConsumer<BookingConfirmedEvent>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ILogger<BookingConfirmedConsumer> _logger;
    private readonly ICacheService _cacheService;
    public BookingConfirmedConsumer(IBookingRepository bookingRepository, ILogger<BookingConfirmedConsumer> logger, ICacheService cacheService)
    {
        _bookingRepository = bookingRepository;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task Consume(ConsumeContext<BookingConfirmedEvent> context)
    {
        try
        {
            _logger.LogInformation("Received BookingConfirmedEvent with ID: {Id}", context.Message.Id);

            var booking = await _bookingRepository.GetByIdBookAsync(context.Message.Id);
            if (booking == null)
            {
                _logger.LogWarning("Booking with ID: {Id} not found", context.Message.Id);
                return;
            }

            var status = Domain.SeedWork.Enumeration.FromDisplayName<BookStatus>(context.Message.Status);

            if (status == BookStatus.Confirmed)
            {
                booking.ConfirmBooking();
                _logger.LogInformation("Booking with ID: {Id} confirmed successfully", context.Message.Id);
            }
            else if (status == BookStatus.Canceled)
            {
                booking.CancelBooking();
                _logger.LogInformation("Booking with ID: {Id} was canceled", context.Message.Id);
            }
            else
            {
                _logger.LogWarning("Invalid status for Booking with ID: {Id}. Status: {Status}",
                    context.Message.Id, context.Message.Status);
                return;
            }

            await _bookingRepository.SaveChangesAsync();

            var cacheKeys = GenerateCacheKeysForBooking(booking);
            foreach (var cacheKey in cacheKeys)
            {
                await _cacheService.RemoveAsync(cacheKey);
            }

            _logger.LogInformation("Cache invalidated for Booking ID: {Id}", context.Message.Id);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing BookingConfirmedEvent with ID: {Id}", context.Message.Id);
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

