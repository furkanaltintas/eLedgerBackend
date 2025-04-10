using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.GetAllCustomers;

class GetAllCustomersQueryHandler(
    ICustomerRepository customerRepository,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<GetAllCustomersQuery, IDomainResult<List<Customer>>>
{
    public async Task<IDomainResult<List<Customer>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        List<Customer> customers = companyContextHelper.GetCompanyFromContext<List<Customer>>("customers");
        if (customers is null)
        {
            customers = await customerRepository.GetAll().OrderBy(c => c.Name).ToListAsync(cancellationToken);
            companyContextHelper.SetCompanyInContext("customers", customers);
        }

        return DomainResult.Success(customers);
    }
}