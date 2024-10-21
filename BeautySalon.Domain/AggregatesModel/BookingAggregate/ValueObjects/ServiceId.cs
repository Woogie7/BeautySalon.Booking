using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects
{
    public class ServiceId : ValueObject
    {
        public Guid Value { get; private set; }

        private ServiceId(Guid id) 
        {
            Value = id;
        }

        public static ServiceId Create(Guid id)
        {
            return new ServiceId(id);
        }

        public static ServiceId CreateUnique()
        {
            return new ServiceId(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

    }
}
