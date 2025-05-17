
namespace BeautySalon.Booking.Application.Interface
{
    public interface IEventBus
    {
        Task SendMessageAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    }
}