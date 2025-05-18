namespace BeautySalon.Booking.Application.Models;

public class ClientReadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string SurnName {  get;  set; }
    public string Email {  get;  set; }
    public string Phone{  get;  set; }
    public DateTime BerthDay{  get;  set; }
}