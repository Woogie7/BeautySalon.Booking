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
        //private readonly ResiliencePipeline<object> _pipeline; 
        //private readonly IPendingQueueRepository _pendingQueueRepository;

        public EventBus(IPublishEndpoint endpoint, ILogger<EventBus> logger, IPendingQueueRepository pendingQueueRepository)
        {
            _endpoint = endpoint;
            _logger = logger;
            /*_pendingQueueRepository = pendingQueueRepository;
            
            _pipeline = new ResiliencePipelineBuilder<object>()
                .AddFallback(new FallbackStrategyOptions<object>
                {
                    ShouldHandle = args => ValueTask.FromResult(true),
                    FallbackAction = async args =>
                    {
                        var contextLogger = args.Context.GetLogger<EventBus>();
                        contextLogger?.LogError(args.Outcome.Exception, "Fallback: Failed to send message");

                        if (args.Context.Properties.TryGetValue(ResilienceKeys.MessageKey, out var message))
                        {
                            contextLogger?.LogWarning("Fallback сработал. Сохраняем сообщение в резерв.");
                            await _pendingQueueRepository.SaveAsync(message);
                        }
                        else
                        {
                            contextLogger?.LogWarning("Message was not found in resilience context.");
                        }

                        return Outcome.FromResult<object?>(default); 
                    },
                    OnFallback = args =>
                    {
                        if (args.Context.Properties.TryGetValue(ResilienceKeys.MessageKey, out var message))
                        {
                            Console.WriteLine($"[Fallback] Message: {JsonConvert.SerializeObject(message)}");
                        }
                        return default;
                    }
                })
                .Build();*/
        }

        public async Task SendMessageAsync<T>(T message, CancellationToken cancellationToken = default)
            where T : class
        {
            // var context = ResilienceContextPool.Shared.Get();
            // context.Properties.Set(ResilienceKeys.MessageKey, message);
            // context.SetLogger(_logger);

            // await _pipeline.ExecuteAsync(async _ =>
            // {
            //     _logger.LogInformation("Sending message to Rabbitmq");
            //     await _endpoint.Publish(message);
            //     return (object?)null;
            // }, context);
            await _endpoint.Publish(message, cancellationToken);
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
    public static class ResilienceKeys
    {
        public static readonly ResiliencePropertyKey<object> MessageKey = new("message");
    }
    
}
