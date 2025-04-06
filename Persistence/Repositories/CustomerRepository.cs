using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class CustomerRepository : Repository<Customer, AppDbContext>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context) { }
}