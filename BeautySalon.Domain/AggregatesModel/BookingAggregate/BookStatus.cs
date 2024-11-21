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
        public static BookStatus Processing = new (1, nameof(Processing));
        public static BookStatus Confirmed = new (1, nameof(Confirmed));
        public static BookStatus Canceled = new (1, nameof(Canceled));

        protected BookStatus(int id, string name) : base(id, name)
        {
        }
    }
}
