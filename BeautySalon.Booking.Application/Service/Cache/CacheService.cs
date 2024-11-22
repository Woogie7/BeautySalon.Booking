using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using BeautySalon.Booking.Application.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;


namespace BeautySalon.Booking.Application.Service.Cache
{
    public class CacheService : ICacheService
    {
        private IDatabase _cacheDb;

        public CacheService(IConfiguration config)
        {
            var cahceConnection = config.GetConnectionString("BookingChache");
            var redis = ConnectionMultiplexer.Connect(cahceConnection);
            _cacheDb = redis.GetDatabase();
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _cacheDb.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }
        public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, TimeSpan expirationTime)
        {
            var cachedData = await GetAsync<T>(key);
            if (cachedData != null)
                return cachedData;

            cachedData = await factory();

            await SetAsync<T>(key, cachedData, expirationTime);

            return cachedData;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            var exist = await _cacheDb.KeyExistsAsync(key);

            if (exist)
                return _cacheDb.KeyDelete(key);

            return false;
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan timeOfDeath)
        {
            string serializedValue = JsonConvert.SerializeObject(value, Formatting.Indented);
            return await _cacheDb.StringSetAsync(key, serializedValue, timeOfDeath);
        }
    }
}
