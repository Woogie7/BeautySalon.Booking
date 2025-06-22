namespace BeautySalon.Booking.Persistence.Repositories;

public interface IPendingQueueRepository
{
    public Task SaveAsync(object message);
}