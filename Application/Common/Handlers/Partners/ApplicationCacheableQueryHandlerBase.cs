using Application.Common.Interfaces;
using DomainResults.Common;

namespace Application.Common.Handlers.Partners;

public abstract class ApplicationCacheableQueryHandlerBase
{
    protected readonly ICacheService CacheService;

    protected ApplicationCacheableQueryHandlerBase(ICacheService cacheService)
    {
        CacheService = cacheService;
    }

    protected async Task<T> GetOrSetCacheAsync<T>(string cacheKey, Func<Task<T>> getDataFunc)
    {
        T? cached = CacheService.Get<T>(cacheKey);
        if (cached is not null) return cached;

        T? data = await getDataFunc();
        CacheService.Set(cacheKey, data);
        return data;
    }

    protected async Task<IDomainResult<T>> Success<T>(string cackeKey, Func<Task<T>> getDataFunc)
    {
        return DomainResult.Success(await GetOrSetCacheAsync(cackeKey, getDataFunc));
    }
}