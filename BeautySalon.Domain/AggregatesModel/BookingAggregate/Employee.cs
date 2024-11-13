using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.SeedWork;
using Newtonsoft.Json;


namespace BeautySalon.Domain.AggregatesModel.BookingAggregate
{
    public class Employee : Entity<EmployeeId>
    {
        public string Name { get; private set; }

        private Employee(EmployeeId employeeId,
            string name)
            : base(employeeId)
        {
            Name = name;
        }

        private Employee() { }

        public static Employee Create(
            string name
            )
        {
            return new(
                EmployeeId.CreateUnique(),
                name);
        }
    }
}
