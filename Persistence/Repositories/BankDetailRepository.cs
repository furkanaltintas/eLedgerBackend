using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class BankDetailRepository : Repository<BankDetail, AppDbContext>, IBankDetailRepository
{
    public BankDetailRepository(AppDbContext context) : base(context) { }
}
