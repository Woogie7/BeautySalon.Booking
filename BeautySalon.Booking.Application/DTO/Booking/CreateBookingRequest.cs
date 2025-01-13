namespace BeautySalon.Booking.Application.DTO.Booking
{
    public record CreateBookingRequest
    (
        DateTime StartTime,
        TimeSpan Duration,
        Guid ServiceId,
        Guid ClientId,
        Guid EmployeeId,
        decimal Discount
    );
}
