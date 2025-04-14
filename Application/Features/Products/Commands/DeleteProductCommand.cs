using DomainResults.Common;
using MediatR;

namespace Application.Features.Products.Commands;

public sealed record DeleteProductCommand(Guid Id) : IRequest<IDomainResult<string>>;