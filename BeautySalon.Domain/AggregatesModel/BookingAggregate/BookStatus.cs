using BeautySalon.Booking.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate
{
    public class BookStatus : Enumeration
    {
        public static readonly BookStatus Processing = new (1, nameof(Processing));
        public static readonly BookStatus Confirmed = new (2, nameof(Confirmed));
        public static readonly BookStatus Canceled = new (3, nameof(Canceled));

        protected BookStatus(int id, string name) : base(id, name)
        {
        }
    }
}
