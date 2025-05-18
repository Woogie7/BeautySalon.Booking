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


        private static readonly TimeOnly SalonOpenTime = new TimeOnly(9, 0);
        private static readonly TimeOnly SalonCloseTime = new TimeOnly(21, 0);
        public BookingTime(DateTime startTime, TimeSpan duration)
        {
            ValidateDuration(duration);
            ValidateStartTime(startTime);
            ValidateSalonWorkingHours(startTime, duration);

            StartTime = startTime;
            Duration = duration;
            EndTime = startTime.Add(duration);
        }

        private void ValidateDuration(TimeSpan duration)
        {
            if (duration <= TimeSpan.FromMinutes(30) || duration >= TimeSpan.FromHours(3))
                throw new DomainException($"Длительность должна быть от 30 минут до 3 часов.");
        }

        private void ValidateStartTime(DateTime startTime)
        {
            if (startTime < DateTime.UtcNow)
                throw new DomainException("Дата бронирования не может быть в прошлом.");
        }

        private void ValidateSalonWorkingHours(DateTime startTime, TimeSpan duration)
        {
            var startTimeOnly = TimeOnly.FromDateTime(startTime);
            var endTimeOnly = TimeOnly.FromDateTime(startTime.Add(duration));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartTime; 
            yield return Duration; 
            yield return EndTime;
        }
    }
}
