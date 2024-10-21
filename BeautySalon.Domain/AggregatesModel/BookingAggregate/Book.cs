using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.Event;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
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
        public EmployeeId EmployeeId { get; private set; }

        public ClientId ClientId { get; private set; }

        public ServiceId ServiceId { get; private set; }


        private bool _isConfirmed;

        private Book(BookId id,
            BookingTime time, 
            EmployeeId employee, 
            ClientId clientId, 
            ServiceId service) : base(id) 
        {
            Time = time;
            EmployeeId = employee;
            ClientId = clientId;
            ServiceId = service;
            _isConfirmed = false;
        }

        private Book() { }

        public static Book Create(
            BookingTime time,
            EmployeeId employeeId,
            ClientId clientId,
            ServiceId serviceId)
        {
            var book = new Book(BookId.CreateUnique(), time, EmployeeId.Create(employeeId.Value), ClientId.Create(clientId.Value), ServiceId.Create(serviceId.Value));

            book.AddDomainEvents(new BookingCreated(book));

            return book;
        }

        public void ConfirmBooking()
        {
            _isConfirmed = true;
        }

    }
}
