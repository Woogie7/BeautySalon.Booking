namespace BeautySalon.Booking.Application.Interface
{
    public interface IEmployeeReedService
    {
        Task<bool> IsEmployeeExistsAsync(Guid employeeId);
    }
}