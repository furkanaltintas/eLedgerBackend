using DomainResults.Common;
using MediatR;

namespace Application.Features.Products.CreateProduct;

public sealed record CreateProductCommand(string Name) : IRequest<IDomainResult<string>>;