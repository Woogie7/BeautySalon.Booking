using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Domain.AggregatesModel.BookingAggregate
{
    public class Service : Entity<ServiceId>
    {
        public string Name { get; }

        private readonly List<Book> _books = new();

        public IReadOnlyList<Book> Books => _books.AsReadOnly();

        private Service(ServiceId serviceId,
            string name)
            : base(serviceId)
        {
            Name = name;
        }

        public static Service Create(
            string name)
        {
            return new(
                ServiceId.CreateUnique(),
                name);
        }
    }
}
