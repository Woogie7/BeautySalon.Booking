namespace BeautySalon.Booking.Application.Models
{
    public class EmployeeReadModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        
        public List<Guid> ServiceIds { get; set; } = new();
        public ICollection<ScheduleReadModel> Schedules { get; set; }
    }
}
