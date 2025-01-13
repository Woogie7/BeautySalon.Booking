using BeautySalon.Booking.Contracts;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Application.Features.Bookings.Confirmed
{
    public class ConfirmBooked : IRequest
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }

    internal class ConfirmedBookingHandler : IRequestHandler<ConfirmBooked>
    {
        private readonly IEventBus _eventBus;

        public ConfirmedBookingHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task Handle(ConfirmBooked request, CancellationToken cancellationToken)
        {
            await _eventBus.SendMessageAsync(new BookingConfirmedEvent
            {
                Id = request.Id,
                Status = request.Status
            });
        }
    }
}
