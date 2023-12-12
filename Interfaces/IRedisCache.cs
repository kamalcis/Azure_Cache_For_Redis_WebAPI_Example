namespace Azure_Cache_For_Redis_WebAPI_Example.Interfaces
{
    public interface IRedisCache
    {
        Task<T> GetCacheData<T>(string key);
        Task<object> RemoveCacheData(string key);

        Task<bool> SetCacheData<T>(string key, T value, DateTimeOffset expirationTime);
        
    }
}
