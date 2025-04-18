using Application.Common.Interfaces;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;

namespace Application.Common.Handlers.Companies;

public abstract class CompanyCommandHandlerBase
{
    protected readonly IUnitOfWorkCompany UnitOfWorkCompany;
    protected readonly ICompanyContextHelper CompanyContextHelper;
    protected readonly IMapper Mapper;

    protected CompanyCommandHandlerBase(
        IUnitOfWorkCompany unitOfWorkCompany,
        ICompanyContextHelper companyContextHelper,
        IMapper mapper)
    {
        UnitOfWorkCompany = unitOfWorkCompany;
        CompanyContextHelper = companyContextHelper;
        Mapper = mapper;
    }

    protected async Task<IDomainResult<T>> SuccessAsync<T>(string[] cacheKeys, T result, CancellationToken cancellationToken = default)
    {
        await UnitOfWorkCompany.SaveChangesAsync(cancellationToken);
        CompanyContextHelper.RemoveRangeCompanyFromContext(cacheKeys);
        return DomainResult.Success(result);
    }
}