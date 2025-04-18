using Application.Common.Interfaces;
using DomainResults.Common;
using MapsterMapper;

namespace Application.Common.Handlers.Companies;

public abstract class CompanyQueryHandlerBase
{
    protected readonly ICompanyContextHelper CompanyContextHelper;
    protected readonly IMapper Mapper;

    protected CompanyQueryHandlerBase(
        ICompanyContextHelper companyContextHelper,
        IMapper mapper)
    {
        CompanyContextHelper = companyContextHelper;
        Mapper = mapper;
    }

    protected IDomainResult<T> Success<T>(string cacheKey, T value, CancellationToken cancellationToken = default)
    {
        CompanyContextHelper.SetCompanyInContext(cacheKey, value);
        return DomainResult.Success(value);
    }

    protected async Task<IDomainResult<T>> GetOrSetCacheAsync<T>(string cacheKey, Func<Task<T>> getDataFunc)
    {
        T? cached = CompanyContextHelper.GetCompanyFromContext<T>(cacheKey);
        if (cached is not null) return DomainResult.Success(cached);

        var data = await getDataFunc();
        return Success(cacheKey, data);
    }
}