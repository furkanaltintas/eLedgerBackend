using Domain.Entities.Partners;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class CompanyRepository : Repository<Company, AppDbContext>, ICompanyRepository
{
    public CompanyRepository(AppDbContext context) : base(context) { }
}