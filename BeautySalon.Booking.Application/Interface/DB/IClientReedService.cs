namespace BeautySalon.Booking.Application.Interface;

public interface IClientReadService
{
    Task<bool> IsClientExistsAsync(Guid clientId);
}