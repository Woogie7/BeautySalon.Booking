namespace BeautySalon.Booking.Application.Interface
{
    public interface IEmployeeService
    {
        Task<bool> IsEmployeeExistsAsync(Guid employeeId);
    }
}