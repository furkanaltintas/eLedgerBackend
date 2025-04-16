using Application.Features.Products.Constants;
using Domain.Entities.Companies;
using Domain.Interfaces;
using DomainResults.Common;

namespace Application.Features.Products.Rules;

public class ProductRules(IProductRepository productRepository)
{
    public async Task<IDomainResult<string>> CheckNameExistsAsync(string name, CancellationToken cancellationToken)
    {
        Boolean isNameExists = await productRepository.AnyAsync(p => p.Name == name, cancellationToken);

        return isNameExists
            ? DomainResult<string>.Failed(ProductsMessages.NameAlreadyExists)
            : DomainResult.Success(string.Empty);
    }

    public async Task<Product> CheckExistsAsync(Guid productId, CancellationToken cancellationToken)
    {
        Product? product = await productRepository.GetByExpressionWithTrackingAsync(
            p => p.Id == productId,
            cancellationToken
        );

        return product ?? null!;
    }
}