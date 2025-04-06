using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class CashRegisterDetailRepository : Repository<CashRegisterDetail, AppDbContext>, ICashRegisterDetailRepository
{
    public CashRegisterDetailRepository(AppDbContext context) : base(context) { }
}