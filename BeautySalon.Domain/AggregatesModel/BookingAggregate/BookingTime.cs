using BeautySalon.Booking.Domain.Exceptions;
using BeautySalon.Domain.SeedWork;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Domain.AggregatesModel.BookingAggregate
{
    public class BookingTime : ValueObject
    {
        public DateTime StartTime { get; private set; }
        public TimeSpan Duration { get; private set; }
        public DateTime EndTime { get; private set; }

        public BookingTime(DateTime startTime, TimeSpan duration)
        {
            if (duration <= TimeSpan.FromMinutes(30) || duration >= TimeSpan.FromHours(3))
            {
                throw new DomainException("Не коректное продолжительность");
            }
            StartTime = startTime;
            Duration = duration;
            EndTime = startTime.Add(Duration);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartTime; 
            yield return Duration; 
            yield return EndTime;
        }
    }
}
