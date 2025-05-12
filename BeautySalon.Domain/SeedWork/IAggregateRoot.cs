using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Domain.SeedWork
{
    public abstract class AggregateRoot<TId> : Entity<TId>
        where TId : notnull
    {
        protected AggregateRoot(TId Id) : base(Id) { }

        protected AggregateRoot() { }
    }
}
