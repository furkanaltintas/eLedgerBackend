using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class BankRepository : Repository<Bank, AppDbContext>, IBankRepository
{
    public BankRepository(AppDbContext context) : base(context) { }
}