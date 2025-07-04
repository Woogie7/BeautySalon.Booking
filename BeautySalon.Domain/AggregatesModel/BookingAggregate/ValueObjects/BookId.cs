﻿using BeautySalon.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects
{
    public sealed class BookId : ValueObject
    {
        public Guid Value { get; private set; }

        private BookId(Guid id)
        {
            Value = id;
        }

        public static BookId CreateUnique()
        {
            return new BookId(Guid.NewGuid());
        }
        public static BookId Create(Guid value)
        {
            return new BookId(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
