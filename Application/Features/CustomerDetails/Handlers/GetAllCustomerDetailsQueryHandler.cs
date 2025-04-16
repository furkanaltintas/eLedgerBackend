using Application.Features.CustomerDetails.Queries;
using Application.Features.Customers.Constants;
using Domain.Entities.Companies;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CustomerDetails.Handlers;

internal sealed class GetAllCustomerDetailsQueryHandler(ICustomerRepository customerRepository) : IRequestHandler<GetAllCustomerDetailsQuery, IDomainResult<Customer>>
{
    public async Task<IDomainResult<Customer>> Handle(GetAllCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
        Customer? customer = await customerRepository.Where(c => c.Id == request.CustomerId)
                                                     .Include(c => c.Details)
                                                     .FirstOrDefaultAsync(cancellationToken);
        
        if (customer is null) return DomainResult.NotFound<Customer>(CustomersMessages.NotFound);
        return DomainResult.Success(customer);
    }
}