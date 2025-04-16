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

class CreateCustomerCommandHandler : CompanyCommandHandlerBase, IRequestHandler<CreateCustomerCommand, IDomainResult<string>>
{
    private readonly ICustomerRepository _customerRepository;
    public CreateCustomerCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, ICustomerRepository customerRepository) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IDomainResult<string>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer customer = Mapper.Map<Customer>(request);
        await _customerRepository.AddAsync(customer, cancellationToken);
        return await SuccessAsync(new[] { CustomersMessages.Cache }, CustomersMessages.Created, cancellationToken);
    }
}