using Application.Features.ProductDetails.Queries;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProductDetails.Handlers;

sealed class GetAllProductDetailsQueryHandler(IProductRepository productRepository) : IRequestHandler<GetAllProductDetailsQuery, IDomainResult<Product>>
{
    public async Task<IDomainResult<Product>> Handle(GetAllProductDetailsQuery request, CancellationToken cancellationToken)
    {
        Product? product = await productRepository.Where(p => p.Id == request.ProductId).Include(p => p.Details).FirstOrDefaultAsync(cancellationToken);
        if(product is null) return DomainResult.NotFound<Product>("Ürün bulunamadı");

        return DomainResult.Success(product);
    }
}

