using Application.Common.Interfaces;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;

namespace Application.Common.Handlers;

public abstract class BaseCommandHandler
{
    protected readonly IUnitOfWorkCompany UnitOfWorkCompany;
    protected readonly ICompanyContextHelper CompanyContextHelper;
    protected readonly IMapper Mapper;

    protected BaseCommandHandler(
        IUnitOfWorkCompany unitOfWorkCompany,
        ICompanyContextHelper companyContextHelper,
        IMapper mapper)
    {
        UnitOfWorkCompany = unitOfWorkCompany;
        CompanyContextHelper = companyContextHelper;
        Mapper = mapper;
    }

    protected async Task<IDomainResult<T>> Success<T>(string[] cacheKey, T result, CancellationToken cancellationToken = default)
    {
        await UnitOfWorkCompany.SaveChangesAsync(cancellationToken);
        CompanyContextHelper.RemoveRangeCompanyFromContext(cacheKey);
        return DomainResult.Success(result);
    }
}