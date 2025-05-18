using AutoMapper;
using BeautySalon.Booking.Application.DTO;
using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Booking.Infrastructure;

public class ClientReadService : IClientReadService
{
    private readonly BookingDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly ILogger<ClientReadService> _logger;

    public ClientReadService(BookingDbContext context, ICacheService cacheService, IMapper mapper, ILogger<ClientReadService> logger)
    {
        _context = context;
        _cacheService = cacheService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<bool> IsClientExistsAsync(Guid clientId)
    {
        var cacheKey = $"client{clientId}";
        var cached = await _cacheService.GetAsync<ClientDTO>(cacheKey);
        if (cached != null)
        {
            return true;
        }
    
        var employee = await _context.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == clientId);

        if (employee != null)
        {
            var dto = _mapper.Map<EmployeeDTO>(employee);
            await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));
        
            return true;
        }
            
        return false;
    }
}