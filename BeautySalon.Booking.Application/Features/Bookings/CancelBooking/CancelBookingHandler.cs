using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Contracts;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Booking.Application.Features.Bookings.CancelBooking
{
    
    internal class CancelBookingHandler : IRequestHandler<CancelBookingCommand, Book>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<CancelBookingHandler> _logger;
        private readonly ICacheService _cacheService;
        private readonly IEventBus _eventBus;

        public CancelBookingHandler(IBookingRepository bookingRepository, ILogger<CancelBookingHandler> logger, ICacheService cacheService, IEventBus eventBus)
        {
            _bookingRepository = bookingRepository;
            _logger = logger;
            _cacheService = cacheService;
            _eventBus = eventBus;
        }

        public async Task<Book> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Canceling book with ID: {Id}", request.Id);

            var booking = await _bookingRepository.GetByIdBookAsync(request.Id);
            if (booking == null)
            {
                _logger.LogWarning("Booking with ID: {Id} not found", request.Id);
                throw new ArgumentException($"Бронь {request.Id} не найдена.");
            }

            if (booking.BookStatus == BookStatus.Confirmed)
            {
                _logger.LogInformation("Booking with ID: {Id} confirmed successfully", request.Id);
                throw new ArgumentException($"Бронь {request.Id} уже потверждена.");
            }
            else if (booking.BookStatus == BookStatus.Canceled)
            {
                _logger.LogInformation("Booking with ID: {Id} was canceled", request.Id);
                throw new ArgumentException($"Бронь {request.Id} уже отменена.");
            }
            else
            {
                booking.CancelBooking();
                _logger.LogWarning("Book status changes to cancel for {Id}", request.Id);
                await _bookingRepository.SaveChangesAsync();

                var cacheKeys = GenerateCacheKeysForBooking(booking);
                foreach (var cacheKey in cacheKeys)
                {
                    await _cacheService.RemoveAsync(cacheKey);
                }
                _logger.LogInformation("Cache invalidated for Booking ID: {Id}", request.Id);

                await _eventBus.SendMessageAsync(
                    new BookingCancelledEvent
                    {
                        BookingId = booking.Id.Value,
                        EmployeeId = booking.EmployeeId.Value,
                        Duration = booking.Time.Duration,
                        StartTime = booking.Time.StartTime
                    });

                return booking;
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
}
