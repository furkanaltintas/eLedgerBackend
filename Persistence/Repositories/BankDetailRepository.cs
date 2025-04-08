using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class BankDetailRepository : Repository<BankDetail, CompanyDbContext>, IBankDetailRepository
{
    public BankDetailRepository(CompanyDbContext context) : base(context) { }
}