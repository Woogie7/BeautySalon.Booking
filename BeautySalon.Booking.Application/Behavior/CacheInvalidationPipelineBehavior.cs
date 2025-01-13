using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Application.Service.Cache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Booking.Application.Behavior
{
    public sealed class CacheInvalidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheInvalidatingCommand<TResponse>
    {
        private readonly ICacheService _cacheService;

        public CacheInvalidationPipelineBehavior(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            foreach (var cacheKey in request.GetCacheKeysToInvalidate())
            {
                await _cacheService.RemoveAsync(cacheKey);
            }

            return response;
        }
    }
}
