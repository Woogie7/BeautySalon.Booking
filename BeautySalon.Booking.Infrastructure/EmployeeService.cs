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
            _logger.LogInformation($"Проверка сотрудника {employeeId} | Ключ кэша: {cacheKey}");

            // 1. Проверка кэша
            var cached = await _cacheService.GetAsync<EmployeeDTO>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation($"Сотрудник {employeeId} найден в кэше");
                return true;
            }

            // 2. Проверка в БД
            _logger.LogInformation($"Сотрудник {employeeId} не найден в кэше, проверяем БД...");
    
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee != null)
            {
                _logger.LogInformation($"Сотрудник {employeeId} найден в БД: {employee.Name}");
        
                var dto = _mapper.Map<EmployeeDTO>(employee);
                await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));
                _logger.LogInformation($"Данные сотрудника {employeeId} сохранены в кэш");
        
                return true;
            }
    
            // 3. Сотрудник не найден
            _logger.LogWarning($"Сотрудник {employeeId} не найден ни в кэше, ни в БД");
            await _cacheService.SetAsync(cacheKey + ":notfound", "1", TimeSpan.FromMinutes(1));
            _logger.LogInformation($"Флаг 'не найдено' для {employeeId} сохранен в кэш");
    
            return false;
        }
    }
}
