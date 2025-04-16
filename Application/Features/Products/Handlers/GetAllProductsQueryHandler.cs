using Application.Common.Handlers.Companies;
using Application.Common.Interfaces;
using Application.Features.Products.Constants;
using Application.Features.Products.Queries;
using Domain.Entities.Companies;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Handlers;

internal sealed class GetAllProductsQueryHandler : CompanyCacheableQueryHandlerBase, IRequestHandler<GetAllProductsQuery, IDomainResult<List<Product>>>
{
    private readonly IProductRepository _productRepository;
    public GetAllProductsQueryHandler(ICompanyContextHelper companyContextHelper, IProductRepository productRepository) : base(companyContextHelper)
    {
        _productRepository = productRepository;
    }

    public async Task<IDomainResult<List<Product>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await Success(ProductsMessages.Cache, async () =>
        {
            return await _productRepository.GetAll()
                                            .OrderBy(p => p.Name)
                                            .ToListAsync(cancellationToken);
        });
    }
}