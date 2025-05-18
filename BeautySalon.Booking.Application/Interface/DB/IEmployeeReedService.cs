using BeautySalon.Booking.Application.Models;

namespace BeautySalon.Booking.Application.Interface.DB
{
    public interface IEmployeeReedService
    {
        Task<EmployeeReadModel> GetEmployeeByIdAsync(Guid employeeId);
        Task<bool> IsEmployeeAvailableAsync(Guid employeeId, DateTime requestedStart, TimeSpan duration);
    }
}