using Core.Application.Caching;
using Core.CrossCuttingConcerns.Exceptions.Middlewares;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Core;

public static class CoreExtensions
{
    public static void AddCoreExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new ConfigurationBuilder().AddJsonFile("serilog.json").Build();
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

        services.Configure<CacheOptions>(configuration.GetSection("CacheConfiguration"));
        services.Configure<RedisOptions>(configuration.GetSection("RedisSettings"));

        services.AddHttpContextAccessor();
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddScoped<ILogModelCreatorService, LogModelCreatorService>();
        services.AddMemoryCache();
        services.AddTransient<MemoryCacheService>();
        services.AddTransient<RedisCacheService>();
        services.AddTransient<Func<CacheType, ICacheService>>(serviceProvider => key =>
        {
            return (key switch
            {
                CacheType.Memory => serviceProvider.GetService<MemoryCacheService>(),
                CacheType.Redis => serviceProvider.GetService<RedisCacheService>(),
                _ => serviceProvider.GetService<RedisCacheService>()
            })!;
        });
    }

    public static void UseCoreMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<LoggerMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();
    }
}