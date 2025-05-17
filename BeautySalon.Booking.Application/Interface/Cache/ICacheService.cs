namespace BeautySalon.Booking.Application.Interface
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task<T> GetAsync<T>(string key, Func<Task<T>> factory, TimeSpan expirationTime);
        Task<bool> RemoveAsync(string key);
        Task<bool> SetAsync<T>(string key, T value, TimeSpan timeOfDeath);
    }
}
