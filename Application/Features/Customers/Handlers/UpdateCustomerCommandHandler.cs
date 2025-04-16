using Application.Common.Handlers.Companies;
using Application.Common.Interfaces;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Constants;
using Domain.Entities.Companies;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Customers.Handlers;

class UpdateCustomerCommandHandler : CompanyCommandHandlerBase, IRequestHandler<UpdateCustomerCommand, IDomainResult<string>>
{
    private readonly ICustomerRepository _customerRepository;
    public UpdateCustomerCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, ICustomerRepository customerRepository) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IDomainResult<string>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer customer = await _customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);
        if (customer is null) return DomainResult<string>.NotFound(CustomersMessages.NotFound);

        Mapper.Map(request, customer);
        return await SuccessAsync(new[] { CustomersMessages.Cache }, CustomersMessages.Updated, cancellationToken);
    }
}