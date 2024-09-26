using BeautySalon.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Domain.AggregatesModel.BookingAggregate
{
    public class Book : Entity, IAggregateRoot
    {
        public BookingTime Time {  get; private set; }

        private Employee _employee;
        public Employee Employee => _employee;

        private Client _client;
        public Client Client => _client;

        public Service _service;
        public Service Service => _service;


        private bool _isConfirmed;

        public Book(BookingTime time, Employee employee, Client client, Service service)
        {
            Time = time;
            _employee = employee;
            _client = client;
            _service = service;

            _isConfirmed = false;
        }

        public void ConfirmBooking()
        {
            _isConfirmed = true;
        }

    }
}
