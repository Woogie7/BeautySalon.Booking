using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Contracts;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using MediatR;

namespace BeautySalon.Booking.Application.Features.Booking.CreateBooking
{
    public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, Book>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmployeeService _employeeService;
        private readonly IEventBus _eventBus;

        public CreateBookingHandler(IBookingRepository bookingRepository, IEmployeeService employeeService, IEventBus messageProducer)
        {
            _bookingRepository = bookingRepository;
            _employeeService = employeeService;
            _eventBus = messageProducer;
        }

        public async Task<Book> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await ValidateClientAndEmployeeAsync(request.ClientId, request.EmployeeId);

                var booking = Book.Create(
                    new BookingTime(request.StartTime, request.Duration),
                    EmployeeId.Create(request.EmployeeId),
                    ClientId.Create(request.ClientId),
                    ServiceId.Create(request.ServiceId)
                    );

                if (await _bookingRepository.IsBusyEmployeeAsync(request.EmployeeId, booking))
                {
                    throw new ArgumentException($"Сотрудник {request.EmployeeId} уже занят в выбранное время.");
                }

                if (await _bookingRepository.IsBusyClientAsync(request.ClientId, booking))
                {
                    throw new ArgumentException($"Клиент {request.ClientId} уже имеет бронирование в выбранное время.");
                }

                await _bookingRepository.CreateAsync(booking);

                await _eventBus.SendMessageAsync(
                    new BookingCreatedEvent
                    {
                        Id = booking.Id.Value,
                        ClientId = booking.ClientId.Value,
                        EmployeeId = booking.EmployeeId.Value,
                        ServiceId = booking.ServiceId.Value,
                        Status = booking.BookStatus.ToString()
                    }, cancellationToken);

                return booking;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Не удалось создать бронирование.'{ex.Message}' Попробуйте еще раз.");
            }

        }

        private async Task ValidateClientAndEmployeeAsync(Guid clientId, Guid employeeId)
        {
            if (!await _employeeService.IsEmployeeExistsAsync(employeeId))
                throw new ArgumentException($"Сотрудник не найден: {employeeId}");

            if (!await _bookingRepository.IsExistClientAsync(clientId))
                throw new ArgumentException($"Клиент не найден: {clientId}");
        }
    }
}
