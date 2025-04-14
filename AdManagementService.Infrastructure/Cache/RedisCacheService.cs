using AdManagementService.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdManagementService.Infrastructure.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync(key);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, JsonSerializer.Serialize(value), expiry);
        }
    }
}
