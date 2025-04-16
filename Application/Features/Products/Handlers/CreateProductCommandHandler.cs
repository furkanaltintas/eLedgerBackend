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

internal sealed class CreateProductCommandHandler : CompanyCommandHandlerBase, IRequestHandler<CreateProductCommand, IDomainResult<string>>
{
    private readonly IProductRepository _productRepository;
    private readonly ProductRules _productRules;

    public CreateProductCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, IProductRepository productRepository, ProductRules productRules) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _productRepository = productRepository;
        _productRules = productRules;
    }

    public async Task<IDomainResult<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        IDomainResult<string> checkNameResult = await _productRules.CheckNameExistsAsync(request.Name, cancellationToken);
        if (!checkNameResult.IsSuccess) return checkNameResult;

        Product product = Mapper.Map<Product>(request);

        await _productRepository.AddAsync(product, cancellationToken);
        return await SuccessAsync(new[] { ProductsMessages.Cache }, ProductsMessages.Created, cancellationToken);
    }
}