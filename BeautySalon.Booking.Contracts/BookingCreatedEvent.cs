using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Contracts
{
    public record BookingCreatedEvent
    {
        public Guid Id { get; init; }
        public Guid ServiceId { get; init; }
        public Guid EmployeeId { get; init; }
        public Guid ClientId { get; init; }
        public string Status { get; init; }
    }
}
