using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using GenericRepository;
using Infrastructure.Services.Cache;
using MapsterMapper;
using MediatR;

namespace Application.Features.Companies.UpdateCompany;

class UpdateCompanyCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService,
    IMapper mapper) : IRequestHandler<UpdateCompanyCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        Company company = await companyRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);
        if(company is null) return DomainResult.NotFound<string>("Şirket bulunamadı");

        if(company.TaxNumber != request.TaxNumber)
        {
            Boolean isTaxNumberExists = await companyRepository.AnyAsync(c => c.TaxNumber == request.TaxNumber, cancellationToken);
            if (isTaxNumberExists) return DomainResult.Conflict<string>("Bu vergi numarasına sahip başka bir şirket bulunmaktadır");
        }

        mapper.Map(request, company);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        cacheService.Remove("companies");

        return DomainResult.Success("Şirket bilgisi başarıyla güncellendi");
    }
}