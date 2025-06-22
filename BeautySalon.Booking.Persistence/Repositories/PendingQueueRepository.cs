using BeautySalon.Booking.Persistence.Context;
using Newtonsoft.Json;

namespace BeautySalon.Booking.Persistence.Repositories;

public class PendingQueueRepository : IPendingQueueRepository
{
    private readonly BookingDbContext _dbContext;

    public PendingQueueRepository(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(object message)
    {

        var pendingQueueMessage = new PendingQueueMessage
        {
            Payload = JsonConvert.SerializeObject(message),
            EntityType = message.GetType().FullName!,
            CreatedAt = DateTime.UtcNow,
            IsPublihed = false
        };
        
        await _dbContext.PendingQueueMessages.AddAsync(pendingQueueMessage);
        await _dbContext.SaveChangesAsync();
    }
}

public class PendingQueueMessage
{
    public Guid Id { get; set; }    
    public string EntityType { get; set; }
    public string Payload { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsPublihed { get; set; }
}