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
        //private readonly IPendingQueueRepository _pendingQueueRepository;

        public EventBus(IPublishEndpoint endpoint, ILogger<EventBus> logger)
        {
            _endpoint = endpoint;
            _logger = logger;

            _pipeline = new ResiliencePipelineBuilder()
            .AddFallback(new FallbackStrategyOptions
            {
                ShouldHandle = args => ValueTask.FromResult(true), 
                FallbackAction = async args =>
                {
                    var contextLogger = args.Context.GetLogger<EventBus>();
                    contextLogger?.LogError(args.Outcome.Exception, "Fallback: Failed to send message");

                    var message = args.Context.Properties.Get<object>("message");
                    if (message is not null)
                    {
                        await _pendingQueueRepository.SaveAsync(message); // твоя реализация
                    }

                    return default!;
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
                _logger.LogInformation("Sending message to Rabbitmq");
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
