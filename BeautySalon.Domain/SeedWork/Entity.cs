using BeautySalon.Booking.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Domain.SeedWork
{
    public abstract class Entity<TId> where TId : notnull
    {

        private readonly List<IDomainEvent> _domainEvents = new();
        
        public TId Id { get; protected set; }

        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;
        protected Entity(TId id)
        {
            Id = id;
        }

        protected Entity() { }

        public void AddDomainEvents(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public override bool Equals(object obj)
        {
            return obj is Entity<TId> entity && Id.Equals(entity.Id);
        }

        public bool Equals(Entity<TId>? other)
        {
            return Equals((object?)other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();

        }
        public static bool operator ==(Entity<TId> left, Entity<TId> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TId> left, Entity<TId> right)
        {
            return !Equals(left, right);
        }
    }
}
