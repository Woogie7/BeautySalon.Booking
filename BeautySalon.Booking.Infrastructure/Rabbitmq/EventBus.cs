using MassTransit;

namespace BeautySalon.Booking.Infrastructure.Rabbitmq
{
    public sealed class EventBus : IEventBus
    {
        private readonly IPublishEndpoint _endpoint;

        public EventBus(IPublishEndpoint endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task SendMessageAsync<T>(T message, CancellationToken cancellationToken = default)
            where T : class
        {
            _endpoint.Publish(message, cancellationToken);
        }
    }
}
