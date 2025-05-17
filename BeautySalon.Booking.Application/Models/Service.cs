namespace BeautySalon.Booking.Application.Models
{
    public class ServiceReadModel
    {
        public Guid Id { get; set; }            
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }  
        public decimal Price { get; set; }      
    }

}
