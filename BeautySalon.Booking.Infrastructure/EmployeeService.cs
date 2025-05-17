using AutoMapper;
using BeautySalon.Booking.Application.DTO;
using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Application.Models;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Booking.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Booking.Application.Service
{
    public class EmployeeReedService : IEmployeeReedService
    {
        private readonly BookingDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeReedService> _logger;

        public EmployeeReedService(BookingDbContext context, ICacheService cacheService, IMapper mapper, ILogger<EmployeeReedService> logger)
        {
            _context = context;
            _cacheService = cacheService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> IsEmployeeExistsAsync(Guid employeeId)
        {
            var cacheKey = $"employee{employeeId}";

            var cached = await _cacheService.GetAsync<EmployeeDTO>(cacheKey);
            if (cached != null)
                return true;

            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == employeeId);
            
            _logger.LogInformation(employee.Name);

            if (employee != null)
            {
                var dto = _mapper.Map<EmployeeDTO>(employee);
                await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));
                _logger.LogInformation("ПИСЬКА");
                return true;
            }
            
            await _cacheService.SetAsync(cacheKey + ":notfound", "1", TimeSpan.FromMinutes(1));
            return false;
        }
    }
}
