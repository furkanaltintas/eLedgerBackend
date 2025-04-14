using Application.Common.Interfaces;
using MapsterMapper;

namespace Application.Common.Handlers;

public abstract class CacheableQueryHandlerBase
{
    protected readonly ICompanyContextHelper CompanyContextHelper;

    protected CacheableQueryHandlerBase(ICompanyContextHelper companyContextHelper)
    {
        CompanyContextHelper = companyContextHelper;
    }

    protected async Task<T> GetOrSetCacheAsync<T>(string key, Func<Task<T>> getDataFunc)
    {
        T? cached = CompanyContextHelper.GetCompanyFromContext<T>(key);
        if (cached is not null) return cached;

        var data = await getDataFunc();
        CompanyContextHelper.SetCompanyInContext(key, data);
        return data;
    }
}
