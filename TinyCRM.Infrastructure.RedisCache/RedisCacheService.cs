using Newtonsoft.Json;
using StackExchange.Redis;
using TinyCRM.Application.Cache.Interface;

namespace TinyCRM.Infrastructure.RedisCache;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _db;
    private readonly IConnectionMultiplexer _redis;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _db = redis.GetDatabase();
    }

    public Task<bool> SetRecordAsync<T>(string key, T data, TimeSpan expireTime)
    {
        var serializedData = JsonConvert.SerializeObject(data);

        return _db.StringSetAsync(key, serializedData, expireTime, When.Always);
    }

    public async Task<T?> GetRecordAsync<T>(string key)
    {
        var stringData = await _db.StringGetAsync(key);
        if (string.IsNullOrWhiteSpace(stringData)) return default;

        return JsonConvert.DeserializeObject<T>(stringData.ToString());
    }

    public Task<bool> RemoveRecordAsync(string key)
    {
        return _db.KeyDeleteAsync(key);
    }

    public async Task ClearAsync()
    {
        var endpoints = _redis.GetEndPoints(true);
        foreach (var endpoint in endpoints)
        {
            var server = _redis.GetServer(endpoint);
            await server.FlushAllDatabasesAsync();
        }
    }
}