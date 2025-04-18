using Domain.Entities.Partners;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class CompanyUserRepository : Repository<CompanyUser, AppDbContext>, ICompanyUserRepository
{
    public CompanyUserRepository(AppDbContext context) : base(context) { }
}