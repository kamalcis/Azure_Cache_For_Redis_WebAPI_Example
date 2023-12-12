using Azure_Cache_For_Redis_WebAPI_Example.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;


namespace Azure_Cache_For_Redis_WebAPI_Example.Repositories
{
    public class RedisCache : IRedisCache
    {
        private IDatabase _db;

        public RedisCache(IConnectionMultiplexer connectionMultiplexer)
        {
            _db = connectionMultiplexer.GetDatabase();
        }
        public async Task<T> GetCacheData<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
               return JsonConvert.DeserializeObject<T>(value);
            }

            return default;
        }

        public async Task<object> RemoveCacheData(string key)
        {
            bool _isKeyExist = await _db.KeyExistsAsync(key);
            if (_isKeyExist)
            {
                return await _db.KeyDeleteAsync(key);
            }

            return false;
        }

        public async Task<bool> SetCacheData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expireTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var _isSet = await _db.StringSetAsync(key, JsonConvert.SerializeObject(value), expireTime);
            return _isSet;

        }
    }
}
