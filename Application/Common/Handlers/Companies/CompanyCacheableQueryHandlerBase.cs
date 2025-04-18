using Application.Common.Interfaces;
using DomainResults.Common;

namespace Application.Common.Handlers.Companies;

public abstract class CompanyCacheableQueryHandlerBase
{
    protected readonly ICompanyContextHelper CompanyContextHelper;

    protected CompanyCacheableQueryHandlerBase(ICompanyContextHelper companyContextHelper)
    {
        CompanyContextHelper = companyContextHelper;
    }

    protected async Task<T> GetOrSetCacheAsync<T>(string cacheKey, Func<Task<T>> getDataFunc)
    {
        T? cached = CompanyContextHelper.GetCompanyFromContext<T>(cacheKey);
        if (cached is not null) return cached;

        var data = await getDataFunc();
        CompanyContextHelper.SetCompanyInContext(cacheKey, data);
        return data;
    }

    protected async Task<IDomainResult<T>> Success<T>(string cackeKey, Func<Task<T>> getDataFunc)
    {
        return DomainResult.Success(await GetOrSetCacheAsync(cackeKey, getDataFunc));
    }
}