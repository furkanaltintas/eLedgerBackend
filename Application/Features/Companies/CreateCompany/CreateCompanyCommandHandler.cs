using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using GenericRepository;
using MapsterMapper;
using MediatR;

namespace Application.Features.Companies.CreateCompany;

class CreateCompanyCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateCompanyCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        Boolean isTaxNumberExists = await companyRepository.AnyAsync(c => c.TaxNumber == request.TaxNumber, cancellationToken);
        if (isTaxNumberExists) return DomainResult.Conflict<string>("Bu vergi numarasına sahip başka bir şirket bulunmaktadır");

        Company company = mapper.Map<Company>(request);
        await companyRepository.AddAsync(company, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return DomainResult.Success("Şirket başarıyla oluşturuldu");
    }
}
