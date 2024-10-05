using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Domain.AggregatesModel.BookingAggregate
{
    public class Book : AggregateRoot<BookId>
    {
        public BookingTime Time {  get; private set; }
        public Employee Employee { get; }

        public Client Client { get; }

        public Service Service { get; }


        private bool _isConfirmed;

        private Book(BookId id,
            BookingTime time, 
            Employee employee, 
            Client clientId, 
            Service service) : base(id) 
        {
            Time = time;
            Employee = employee;
            Client = clientId;
            Service = service;
            _isConfirmed = false;
        }

        public static Book Create(
            BookingTime time,
            Employee employee,
            Client client,
            Service service)
        {
            return new(BookId.CreateUnique(), time, employee, client, service);
        }

        public void ConfirmBooking()
        {
            _isConfirmed = true;
        }

    }
}
