using BeautySalon.Domain.SeedWork;

namespace BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects
{
    public sealed class ClinetId : ValueObject
    {
        public Guid Value { get; }

        private ClinetId(Guid id) 
        {
            Value = id;
        }

        public static ClinetId CreateUnique()
        {
            return new ClinetId(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
