using DomainResults.Common;
using MediatR;

namespace Application.Features.Products.Commands;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name) : IRequest<IDomainResult<string>>;