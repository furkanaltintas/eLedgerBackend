using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class ProductRepository : Repository<Product, AppDbContext>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }
}