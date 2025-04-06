using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class ProductDetailRepository : Repository<ProductDetail, AppDbContext>, IProductDetailRepository
{
    public ProductDetailRepository(AppDbContext context) : base(context) { }
}