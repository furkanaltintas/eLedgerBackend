using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Products.UpdateProduct;

sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper,
    IMapper mapper) : IRequestHandler<UpdateProductCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product? product = await productRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);
        if(product is null) return DomainResult.NotFound<string>($"Product with id {request.Id} not found.");

        if (product.Name != request.Name)
        {
            Boolean isNameExists = await productRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);
            if (isNameExists) return DomainResult.Failed<string>("Product with the same name already exists.");
        }

        mapper.Map(request, product);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveCompanyFromContext("products");

        return DomainResult.Success("Product updated successfully.");
    }
}