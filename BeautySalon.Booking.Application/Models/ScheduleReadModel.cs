namespace BeautySalon.Booking.Application.Models;

public class ScheduleReadModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}
