using Core.Application.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Core.Test.Helper;

public static class CacheMockHelper
{
    public static Func<CacheType, ICacheService> GetCacheService()
    {
        var cacheService = new Func<CacheType, ICacheService>(cacheType =>
        {
            ICacheService cacheService;

            switch (cacheType)
            {
                case CacheType.Redis:
                    var redisOptions = Options.Create(new RedisOptions
                    {
                        Host = "172.21.112.1",
                        Port = "6379",
                        DefaultDatabase = "0",
                        InstanceName = "myRedisInstance"
                    });
                    cacheService = new RedisCacheService(redisOptions);
                    break;

                case CacheType.Memory:
                    var cacheOptions = new CacheOptions
                    {
                        AbsoluteExpirationInHours = 1,
                        SlidingExpirationInMinutes = 30
                    };
                    var memoryCacheOptions = Options.Create(cacheOptions);
                    var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
                    cacheService = new MemoryCacheService(memoryCache, memoryCacheOptions);
                    break;

                default:
                    throw new ArgumentException("Invalid cache type.");
            }

            return cacheService;
        });

        return cacheService;
    }
}