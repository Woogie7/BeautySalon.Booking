using AutoMapper;
using BeautySalon.Booking.Application.DTO;
using BeautySalon.Booking.Application.Interface;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Application.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IBookingRepository _bookRepository;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public EmployeeService(IBookingRepository bookRepository, ICacheService cacheService, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<bool> IsEmployeeExistsAsync(Guid employeeId)
            {   
                var cacheEmployee = await _cacheService.GetAsync<EmployeeDTO>($"employee{employeeId}");
                if (cacheEmployee != null)
                    return true;

                // var databaseEmployee = await _bookRepository.GetByIdEmployeeAsync(employeeId);
                // if (databaseEmployee != null)
                // {
                //     var employeeDto = _mapper.Map<EmployeeDTO>(databaseEmployee);
                //     await _cacheService.SetAsync($"employee{employeeId}", employeeDto, TimeSpan.FromMinutes(5));
                //     return true;
                // }

                // Запрос в RabbitMq

                return false;
            }
    }
}
