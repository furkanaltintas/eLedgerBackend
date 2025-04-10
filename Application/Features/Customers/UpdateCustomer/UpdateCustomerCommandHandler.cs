using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Customers.UpdateCustomer;

class UpdateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    IMapper mapper,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<UpdateCustomerCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);
        if (customer is null) return DomainResult<string>.NotFound("Customer not found.");

        mapper.Map(request, customer);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveCompanyFromContext("customers");

        return DomainResult.Success("Customer updated successfully.");
    }
}