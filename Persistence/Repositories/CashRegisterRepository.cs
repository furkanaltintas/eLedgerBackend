using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

public class CashRegisterRepository : Repository<CashRegister, CompanyDbContext>, ICashRegisterRepository
{
    public CashRegisterRepository(CompanyDbContext context) : base(context) { }
}