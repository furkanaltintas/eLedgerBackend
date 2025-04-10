using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CustomerDetails.GetAllCustomerDetails;

class GetAllCustomerDetailsQueryHandler(ICustomerRepository customerRepository) : IRequestHandler<GetAllCustomerDetailsQuery, IDomainResult<Customer>>
{
    public async Task<IDomainResult<Customer>> Handle(GetAllCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
        Customer? customer = await customerRepository.Where(c => c.Id == request.CustomerId).Include(c => c.Details).FirstOrDefaultAsync(cancellationToken);
        if (customer == null) return DomainResult.NotFound<Customer>("Customer not found");
        return DomainResult.Success(customer);
    }
}