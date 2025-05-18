using AutoMapper;
using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Application.Interface.DB;
using BeautySalon.Booking.Application.Models;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Booking.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Booking.Infrastructure
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

        public async Task<EmployeeReadModel?> GetEmployeeByIdAsync(Guid employeeId)
        {
            var cacheKey = $"employee{employeeId}";
            var cached = await _cacheService.GetAsync<EmployeeReadModel>(cacheKey);
            if (cached != null)
            {
                return cached;
            }

            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee != null)
            {
                await _cacheService.SetAsync(cacheKey, employee, TimeSpan.FromMinutes(5));
                return employee;
            }

            await _cacheService.SetAsync(cacheKey + ":notfound", "1", TimeSpan.FromMinutes(1));
            return null;
        }
        
        public async Task<bool> IsEmployeeAvailableAsync(Guid employeeId, DateTime requestedStart, TimeSpan duration)
        {
            var requestedEnd = requestedStart.Add(duration);
            var dayOfWeek = requestedStart.DayOfWeek;

            var schedule = await _context.Schedules
                .Where(s => s.EmployeeId == employeeId && s.DayOfWeek == dayOfWeek)
                .ToListAsync();

            if (!schedule.Any())
                return false;
            
            var timeOfDayStart = requestedStart.TimeOfDay;
            var timeOfDayEnd = requestedEnd.TimeOfDay;

            var fitsSchedule = schedule.Any(s =>
                s.StartTime <= timeOfDayStart &&
                s.EndTime >= timeOfDayEnd
            );

            if (!fitsSchedule)
                return false;
            
            var overlappingBooking = await _context.Books
                .AnyAsync(b =>
                    b.EmployeeId ==  EmployeeId.Create(employeeId) &&
                    ((requestedStart >= b.Time.StartTime && requestedStart < b.Time.EndTime) ||
                     (requestedEnd > b.Time.StartTime && requestedEnd <= b.Time.EndTime) ||
                     (requestedStart <= b.Time.StartTime && requestedEnd >= b.Time.EndTime))
                );

            return !overlappingBooking;
        }



    }
}
