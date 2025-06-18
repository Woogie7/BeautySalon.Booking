using BeautySalon.Booking.Application.Exceptions;
using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Application.Interface.DB;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Booking.Domain.Exceptions;
using BeautySalon.Contracts;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;

namespace BeautySalon.Booking.Application.Features.Bookings.CreateBooking
{
    public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, Book>
    {
        private readonly ILogger<CreateBookingHandler> _logger;
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmployeeReedService _employeeService;
        private readonly IClientReadService _clientService;
        private readonly IEventBus _eventBus;

        public CreateBookingHandler(IBookingRepository bookingRepository, IEmployeeReedService employeeService, IEventBus messageProducer, ILogger<CreateBookingHandler> logger, IClientReadService clientService)
        {
            _bookingRepository = bookingRepository;
            _employeeService = employeeService;
            _eventBus = messageProducer;
            _logger = logger;
            _clientService = clientService;
        }

        public async Task<Book> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await ValidateClientAndEmployeeAsync(request.ClientId, request.EmployeeId, request.ServiceId);

                var booking = Book.Create(
                    new BookingTime(request.StartTime, request.Duration),
                    EmployeeId.Create(request.EmployeeId),
                    ClientId.Create(request.ClientId),
                    ServiceId.Create(request.ServiceId)
                );

                if (!await _employeeService.IsEmployeeAvailableAsync(
                        request.EmployeeId, request.StartTime, request.Duration))
                {
                    throw new BadRequestException("Сотрудник недоступен в выбранное время");
                }


                if (await _bookingRepository.IsBusyClientAsync(request.ClientId, booking))
                {
                    throw new ConflictException($"Клиент {request.ClientId} уже имеет бронирование в выбранное время.");
                }

                await _bookingRepository.CreateAsync(booking);
                
                await _eventBus.SendMessageAsync(new BookingSlotReservedEvent
                {
                    EmployeeId = booking.EmployeeId.Value,
                    BookingId = booking.Id.Value,
                    StartTime = booking.Time.StartTime,
                    Duration = booking.Time.Duration
                }, cancellationToken);


                return booking;
            }
            catch (DomainException ex)
            {
                throw new BadRequestException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании бронирования");
                throw;
            }

        }

        private async Task ValidateClientAndEmployeeAsync(Guid clientId, Guid employeeId, Guid serviceId)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
                throw new NotFoundException($"Сотрудник не найден: {employeeId}");

            if (!employee.ServiceIds.Contains(serviceId))
                throw new BadRequestException($"Сотрудник не оказывает услугу {serviceId}");

            var clientExists = await _clientService.IsClientExistsAsync(clientId);
            if (!clientExists)
                throw new NotFoundException($"Клиент не найден: {clientId}");
        }

    }
}
