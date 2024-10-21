using BeautySalon.Domain.SeedWork;

namespace BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects
{
    public sealed class ClientId : ValueObject
    {
        public Guid Value { get; private set; }

        private ClientId(Guid id) 
        {
            Value = id;
        }

        public static ClientId Create(Guid id)
        {
            return new ClientId(id);
        }

        public static ClientId CreateUnique()
        {
            return new ClientId(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
