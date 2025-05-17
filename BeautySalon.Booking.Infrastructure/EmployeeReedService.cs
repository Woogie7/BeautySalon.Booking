using AutoMapper;
using BeautySalon.Booking.Application.DTO;
using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Application.Models;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Booking.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Booking.Application.Service
{
    public class EmployeeReedService : IEmployeeReedService
    {
        private readonly BookingDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public EmployeeReedService(BookingDbContext context, ICacheService cacheService, IMapper mapper)
        {
            _context = context;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<bool> IsEmployeeExistsAsync(Guid employeeId)
            {   
                var cacheEmployee = await _cacheService.GetAsync<EmployeeDTO>($"employee{employeeId}");
                if (cacheEmployee != null)
                    return true;

                var databaseEmployee = await GetByIdEmployeeAsync(employeeId);
                if (databaseEmployee != null)
                {
                    var employeeDto = _mapper.Map<EmployeeDTO>(databaseEmployee);
                    await _cacheService.SetAsync($"employee{employeeId}", employeeDto, TimeSpan.FromMinutes(5));
                    return true;
                }
                

                return false;
            }
        
        public async Task<EmployeeReadModel?> GetByIdEmployeeAsync(Guid employeeId)
        {
        
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == employeeId);
            return employee;
        }
    }
}
