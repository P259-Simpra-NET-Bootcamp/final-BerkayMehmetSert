using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Core.Application.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _redisDatabase;
    private readonly string _instanceName;

    public RedisCacheService(IOptions<RedisOptions> redisOptions)
    {
        _instanceName = redisOptions.Value.InstanceName;
        var configurationOptions = new ConfigurationOptions
        {
            EndPoints = { $"{redisOptions.Value.Host}:{redisOptions.Value.Port}" },
            DefaultDatabase = int.Parse(redisOptions.Value.DefaultDatabase)
        };
        var redisConnection = ConnectionMultiplexer.Connect(configurationOptions);
        _redisDatabase = redisConnection.GetDatabase();
    }

    // Bu Method ile Cache'de var mı yok mu kontrol ediyoruz.
    public bool TryGet<T>(string cacheKey, out T value)
    {
        var redisValue = _redisDatabase.StringGet(FormatCacheKey(cacheKey));
        if (!redisValue.IsNull)
        {
            value = Deserialize<T>(redisValue);
            return true;
        }

        value = default;
        return false;
    }

    public T Set<T>(string cacheKey, T value)
    {
        _redisDatabase.StringSet(FormatCacheKey(cacheKey), Serialize(value));
        return value;
    }

    public void Remove(string cacheKey) => _redisDatabase.KeyDelete(FormatCacheKey(cacheKey));
    private string FormatCacheKey(string cacheKey) => $"{_instanceName}:{cacheKey}";
    private string Serialize(object value) => JsonConvert.SerializeObject(value);
    private T Deserialize<T>(RedisValue redisValue) => JsonConvert.DeserializeObject<T>(redisValue);
}