using Application.Common.Handlers.Companies;
using Application.Common.Interfaces;
using Application.Features.Products.Commands;
using Application.Features.Products.Constants;
using Application.Features.Products.Rules;
using Domain.Entities.Companies;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Products.Handlers;

internal sealed class DeleteProductCommandHandler : CompanyCommandHandlerBase, IRequestHandler<DeleteProductCommand, IDomainResult<string>>
{
    private readonly IProductRepository _productRepository;
    private readonly ProductRules _productRules;

    public DeleteProductCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, IProductRepository productRepository, ProductRules productRules) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _productRepository = productRepository;
        _productRules = productRules;
    }

    public async Task<IDomainResult<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _productRules.CheckExistsAsync(request.Id, cancellationToken);
        if(product is null) return DomainResult.NotFound<string>(ProductsMessages.NotFound);
        product.IsDeleted = true;

        return await SuccessAsync(new[] { ProductsMessages.Cache }, ProductsMessages.Deleted, cancellationToken);
    }
}