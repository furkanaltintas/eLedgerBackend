using Application.Common.Interfaces;
using DomainResults.Common;
using MapsterMapper;

namespace Application.Common.Handlers;

public abstract class BaseQueryHandler
{
    protected readonly ICompanyContextHelper CompanyContextHelper;
    protected readonly IMapper Mapper;

    protected BaseQueryHandler(
        ICompanyContextHelper companyContextHelper,
        IMapper mapper)
    {
        CompanyContextHelper = companyContextHelper;
        Mapper = mapper;
    }

    protected IDomainResult<T> Success<T>(string cacheKey, T values, CancellationToken cancellationToken = default)
    {
        CompanyContextHelper.SetCompanyInContext(cacheKey, values);
        return DomainResult.Success(values);
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