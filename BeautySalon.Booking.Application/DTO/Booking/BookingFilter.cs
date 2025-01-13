using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Application.DTO.Booking
{
    public class BookingFilter
    {
        public Guid? ClientId {  get; set; }
        public Guid? EmployeeId { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
