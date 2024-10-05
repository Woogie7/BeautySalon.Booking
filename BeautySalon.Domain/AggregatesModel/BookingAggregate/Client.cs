using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.SeedWork;

namespace BeautySalon.Domain.AggregatesModel.BookingAggregate
{
    public sealed class Client : Entity<ClinetId>
    {
        public string Name {  get; }
        public string SurnName {  get; }
        public string Email {  get; }
        public int Phone{  get; }
        public DateTime BerthDay{  get; }
        private readonly List<Book> _books = new();

        public IReadOnlyList<Book> Books => _books.AsReadOnly();

        private Client(ClinetId clientId, 
            string name, 
            string surnName, 
            string email, 
            int phone, 
            DateTime berthDay) 
            : base(clientId)
        {
            Name = name;
            SurnName = surnName;
            Email = email;
            Phone = phone;
            BerthDay = berthDay;
        }

        public static Client Create(
            string name,
            string surnName,
            string email,
            int phone,
            DateTime berthDay
            )
        {
            return new(
                ClinetId.CreateUnique(), 
                name, 
                surnName, 
                email, 
                phone, 
                berthDay);
        }
    }
}
