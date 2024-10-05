using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects
{
    public class EmployeeId : ValueObject
    {
        public Guid Value { get; }

        private EmployeeId(Guid id)
        {
            Value = id;
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
