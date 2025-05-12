using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Contracts
{
    public record BookingCanceledEvent
    {
        public Guid Id { get; init; }
        public string Reason { get; init; }
    }
}
