using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class ProductDetailRepository : Repository<ProductDetail, CompanyDbContext>, IProductDetailRepository
{
    public ProductDetailRepository(CompanyDbContext context) : base(context) { }
}