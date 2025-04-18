using Domain.Entities.Companies;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class UnitRepository: Repository<Unit, CompanyDbContext>, IUnitRepository
    {
        public UnitRepository(CompanyDbContext context) : base(context)
        {
        }
    }
}
