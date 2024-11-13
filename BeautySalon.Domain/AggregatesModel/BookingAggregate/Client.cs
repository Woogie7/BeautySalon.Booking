using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.SeedWork;

namespace BeautySalon.Domain.AggregatesModel.BookingAggregate
{
    public sealed class Client : Entity<ClientId>
    {
        public string Name { get; private set; }
        public string SurnName {  get; private set; }
        public string Email {  get; private set; }
        public string Phone{  get; private set; }
        public DateTime BerthDay{  get; private set; }


        private Client(ClientId clientId, 
            string name, 
            string surnName, 
            string email, 
            string phone, 
            DateTime berthDay) 
            : base(clientId)
        {
            Name = name;
            SurnName = surnName;
            Email = email;
            Phone = phone;
            BerthDay = berthDay;
        }

        private Client() { }    

        public static Client Create(
            string name,
            string surnName,
            string email,
            string phone,
            DateTime berthDay
            )
        {
            return new(
                ClientId.CreateUnique(), 
                name, 
                surnName, 
                email, 
                phone, 
                berthDay);
        }
    }
}
