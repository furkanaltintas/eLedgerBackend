using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using GenericRepository;
using Infrastructure.Services.Cache;
using MediatR;

namespace Application.Features.Companies.DeleteCompany;

class DeleteCompanyCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<DeleteCompanyCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        Company company = await companyRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);
        if (company is null) return DomainResult.NotFound<string>("Şirket bulunamadı");

        company.IsDeleted = true;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        cacheService.Remove("companies");

        return DomainResult.Success("Şirket başarıyla silindi");
    }
}