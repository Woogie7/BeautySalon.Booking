namespace BeautySalon.Booking.Application.Models;

public class EmployeeAvailability
{
    public Guid Id { get; set; }              
    public Guid EmployeeId { get; set; }  
    public EmployeeReadModel Employee { get; set; }
    public DateTime StartTime { get; set; }  
    public DateTime EndTime { get; set; }
}

