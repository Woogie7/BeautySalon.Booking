using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Contracts;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Booking.Application.Features.Bookings.CreateBooking;

public sealed class BookingConfirmedConsumer : IConsumer<BookingConfirmedEvent>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ILogger<BookingConfirmedConsumer> _logger;
    public BookingConfirmedConsumer(IBookingRepository bookingRepository, ILogger<BookingConfirmedConsumer> logger)
    {
        _bookingRepository = bookingRepository;
        _logger = logger;
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

            var status = BookStatus.FromDisplayName<BookStatus>(context.Message.Status);

            if (status == BookStatus.Confirmed)
            {
                booking.ConfirmBooking();
                await _bookingRepository.SaveChangesAsync();

                _logger.LogInformation("Booking with ID: {Id} confirmed successfully", context.Message.Id);

                //Нужно отправить уведомление о потверждение заказа
            }

            if (status == BookStatus.Canceled)
            {
                booking.CanceledBooking();
                await _bookingRepository.SaveChangesAsync();

                _logger.LogInformation("Booking with ID: {Id} it was Canceled", context.Message.Id);

                //Нужно отправить уведомление о потверждение заказа
            }

            else
            {
                _logger.LogWarning("Invalid status for Booking with ID: {Id}. Status: {Status}",
                context.Message.Id, context.Message.Status);
            }
        }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error processing BookingConfirmedEvent with ID: {Id}", context.Message.Id);
    }

    }
}
