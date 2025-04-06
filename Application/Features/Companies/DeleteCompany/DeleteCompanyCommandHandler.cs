using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using GenericRepository;
using MediatR;

namespace Application.Features.Companies.DeleteCompany;

class DeleteCompanyCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteCompanyCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        Company company = await companyRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);
        if (company is null) return DomainResult.NotFound<string>("Şirket bulunamadı");

        company.IsDeleted = true;
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return DomainResult.Success("Şirket başarıyla silindi");
    }
}