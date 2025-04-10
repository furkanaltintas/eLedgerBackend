using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Products.CreateProduct;

class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper,
    IMapper mapper) : IRequestHandler<CreateProductCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Boolean isNameExists = await productRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);
        if(isNameExists) return DomainResult.Failed<string>("Product with the same name already exists.");

        Product product = mapper.Map<Product>(request);

        await productRepository.AddAsync(product, cancellationToken);
        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveCompanyFromContext("products");
        return DomainResult.Success("Product created successfully.");
    }
}