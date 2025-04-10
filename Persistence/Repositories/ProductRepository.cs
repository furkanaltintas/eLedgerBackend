using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class ProductRepository : Repository<Product, CompanyDbContext>, IProductRepository
{
    public ProductRepository(CompanyDbContext context) : base(context) { }
}