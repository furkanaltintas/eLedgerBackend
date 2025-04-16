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

internal sealed class UpdateProductCommandHandler : CompanyCommandHandlerBase, IRequestHandler<UpdateProductCommand, IDomainResult<string>>
{
    private readonly ProductRules _productRules;

    public UpdateProductCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, ProductRules productRules) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _productRules = productRules;
    }

    public async Task<IDomainResult<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product product = await _productRules.CheckExistsAsync(request.Id, cancellationToken);
        if(product is null) return DomainResult.NotFound<string>(ProductsMessages.NotFound);

        if (product.Name != request.Name)
        {
            IDomainResult<string> checkNameExistsResult = await _productRules.CheckNameExistsAsync(request.Name, cancellationToken);
            if(!checkNameExistsResult.IsSuccess) return checkNameExistsResult;
        }

        Mapper.Map(request, product);

        return await SuccessAsync(new[] { ProductsMessages.Cache}, ProductsMessages.Updated, cancellationToken);
    }
}