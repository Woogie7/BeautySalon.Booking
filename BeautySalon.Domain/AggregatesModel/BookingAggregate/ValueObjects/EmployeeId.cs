using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.SeedWork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects
{
    public class EmployeeId : ValueObject
    {
        public Guid Value { get; private set; }

        private EmployeeId(Guid id)
        {
            Value = id;
        }

        public static EmployeeId Create(Guid id)
        {
            return new EmployeeId(id);
        }

        public static EmployeeId CreateUnique()
        {
            return new EmployeeId(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
