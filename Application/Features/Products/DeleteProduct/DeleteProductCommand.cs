using DomainResults.Common;
using MediatR;

namespace Application.Features.Products.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id) : IRequest<IDomainResult<string>>;