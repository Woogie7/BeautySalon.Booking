using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
