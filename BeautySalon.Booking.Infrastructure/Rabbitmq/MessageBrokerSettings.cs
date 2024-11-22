namespace BeautySalon.Booking.Infrastructure.Rabbitmq
{
    public sealed class MessageBrokerSettings
    {
        public string Host {  get; set; } = string.Empty;
        public string UserName {  get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
    }
}