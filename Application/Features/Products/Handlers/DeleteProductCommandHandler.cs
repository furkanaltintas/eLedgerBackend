using Application.Common.Interfaces;
using Application.Features.Products.Commands;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Products.Handlers;

sealed class DeleteProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<DeleteProductCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        Product? product = await productRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);
        if (product is null) return DomainResult.NotFound<string>($"Product with id {request.Id} not found.");

        product.IsDeleted = true;

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveCompanyFromContext("products");

        return DomainResult.Success("Product deleted successfully.");
    }
}