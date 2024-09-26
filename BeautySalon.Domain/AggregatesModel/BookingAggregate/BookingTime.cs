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
        public DateTime EndTime { get; private set; }

        public BookingTime(DateTime startTime, DateTime endTime)
        {
            if (endTime <= startTime)
                throw new ArgumentException("EndTime must be later than start time");
            StartTime = startTime;
            EndTime = endTime;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartTime; 
            yield return EndTime;
        }
    }
}
