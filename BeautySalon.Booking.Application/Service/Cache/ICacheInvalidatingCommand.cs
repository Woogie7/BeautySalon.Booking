using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Application.Service.Cache
{
    public interface ICacheInvalidatingCommand<TResponse> : IRequest<TResponse>
    {
        IEnumerable<string> GetCacheKeysToInvalidate();
    }
}
