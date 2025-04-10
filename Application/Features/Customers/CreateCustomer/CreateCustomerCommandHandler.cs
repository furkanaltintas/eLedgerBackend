using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Customers.CreateCustomer;

class CreateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper,
    IMapper mapper) :IRequestHandler<CreateCustomerCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer customer = mapper.Map<Customer>(request);

        await customerRepository.AddAsync(customer, cancellationToken);
        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveCompanyFromContext("customers");

        return DomainResult.Success("Customer created successfully");
    }
}