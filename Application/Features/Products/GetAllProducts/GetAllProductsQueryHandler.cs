using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.GetAllProducts;

sealed class GetAllProductsQueryHandler(
    IProductRepository productRepository,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<GetAllProductsQuery, IDomainResult<List<Product>>>
{
    public async Task<IDomainResult<List<Product>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        List<Product>? products = companyContextHelper.GetCompanyFromContext<List<Product>>("products");
        if(products is null)
        {
            products = await productRepository.GetAll().OrderBy(p => p.Name).ToListAsync(cancellationToken);
            companyContextHelper.SetCompanyInContext("products", products);
        }

        return DomainResult.Success(products);
    }
}

