using Application.Common.Handlers.Companies;
using Application.Common.Interfaces;
using Application.Features.Customers.Constants;
using Application.Features.Customers.Queries;
using Domain.Entities.Companies;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.Handlers;

class GetAllCustomersQueryHandler : CompanyQueryHandlerBase, IRequestHandler<GetAllCustomersQuery, IDomainResult<List<Customer>>>
{
    private readonly ICustomerRepository _customerRepository;
    public GetAllCustomersQueryHandler(ICompanyContextHelper companyContextHelper, IMapper mapper, ICustomerRepository customerRepository) : base(companyContextHelper, mapper)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IDomainResult<List<Customer>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        return await GetOrSetCacheAsync(CustomersMessages.Cache, async () =>
        {
            return await _customerRepository.GetAll()
                                            .OrderBy(c => c.Name)
                                            .ToListAsync(cancellationToken);
        });
    }
}