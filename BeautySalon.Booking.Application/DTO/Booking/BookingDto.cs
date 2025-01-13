namespace BeautySalon.Booking.Application.DTO.Booking
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string ClientName { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
    }
}
