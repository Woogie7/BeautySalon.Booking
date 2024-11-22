
namespace BeautySalon.Booking.Infrastructure.Rabbitmq
{
    public interface IEventBus
    {
        Task SendMessageAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    }
}