using Domain.Entities.Companies;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

public class CategoryRepository : Repository<Category, CompanyDbContext>, ICategoryRepository
{
    public CategoryRepository(CompanyDbContext context) : base(context)
    {
    }
}