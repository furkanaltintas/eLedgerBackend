using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class CustomerDetailRepository : Repository<CustomerDetail, AppDbContext>, ICustomerDetailRepository
{
    public CustomerDetailRepository(AppDbContext context) : base(context) { }
}