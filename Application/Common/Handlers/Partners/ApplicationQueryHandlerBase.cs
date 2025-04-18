using Application.Common.Interfaces;
using DomainResults.Common;
using MapsterMapper;

namespace Application.Common.Handlers.Partners;

public abstract class ApplicationQueryHandlerBase
{
    protected readonly ICacheService CacheService;
    protected readonly IMapper Mapper;

    protected ApplicationQueryHandlerBase(ICacheService cacheService, IMapper mapper)
    {
        CacheService = cacheService;
        Mapper = mapper;
    }

    protected IDomainResult<T> Success<T>(string cacheKey, T value, CancellationToken cancellationToken = default)
    {
        CacheService.Set(cacheKey, value);
        return DomainResult.Success(value);
    }

    protected async Task<IDomainResult<T>> GetOrSetCacheAsync<T>(string cacheKey, Func<Task<T>> getDataFunc)
    {
        T? cached = CacheService.Get<T>(cacheKey);
        if (cached is not null) return DomainResult.Success(cached);

        T? data = await getDataFunc();
        return Success(cacheKey, cached);
    }
}