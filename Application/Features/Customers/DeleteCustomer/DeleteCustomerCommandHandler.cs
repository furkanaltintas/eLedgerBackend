using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Customers.DeleteCustomer;

class DeleteCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<DeleteCustomerCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);
        if (customer is null) return DomainResult<string>.NotFound("Customer not found.");

        customer.IsDeleted = true;

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveCompanyFromContext("customers");

        return DomainResult.Success("Customer deleted successfully.");
    }
}

