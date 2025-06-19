using System.Runtime.InteropServices.ComTypes;
using BeautySalon.Booking.Application.Interface;
using BeautySalon.Booking.Persistence.Repositories;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;

namespace BeautySalon.Booking.Infrastructure.Rabbitmq
{
    public sealed class EventBus : IEventBus
    {
        private readonly IPublishEndpoint _endpoint;
        private readonly ILogger<EventBus> _logger;
        private readonly ResiliencePipeline _pipeline;
        private readonly IPendingQueueRepository _pendingQueueRepository;

        public EventBus(IPublishEndpoint endpoint, ILogger<EventBus> logger, IPendingQueueRepository pendingQueueRepository)
        {
            _endpoint = endpoint;
            _logger = logger;
            _pendingQueueRepository = pendingQueueRepository;

            _pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry =  args =>
                {
                    var contextLogger = args.Context.GetLogger<EventBus>();
                    contextLogger?.LogWarning(args.Outcome.Exception, "Retry #{0} for sending message", args.AttemptNumber);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
        }

        public async Task SendMessageAsync<T>(T message, CancellationToken cancellationToken = default)
            where T : class
        {
            var context = ResilienceContextPool.Shared.Get();
            //context.Properties.Set("message", message);
            context.SetLogger(_logger);
            //context.SetPendingQueueRepository(_pendingQueueRepository);

            await _pipeline.ExecuteAsync(async _ =>
            {
                await _endpoint.Publish(message);
            }, context);
        }

    }
    public static class LogHelper
    {
        private static readonly ResiliencePropertyKey<ILogger> LoggerKey = new("logger");

        public static void SetLogger<T>(this ResilienceContext context, ILogger<T> logger)
        {
            context.Properties.Set(LoggerKey, logger);
        }

        public static ILogger<T>? GetLogger<T>(this ResilienceContext context)
        {
            if (context.Properties.TryGetValue(LoggerKey, out var loggerObj) && loggerObj is ILogger<T> logger)
            {
                return logger;
            }

            return null;
        }
    }


    
}
