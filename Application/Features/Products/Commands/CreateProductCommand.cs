using DomainResults.Common;
using MediatR;

namespace Application.Features.Products.Commands;

public sealed record CreateProductCommand(string Name) : IRequest<IDomainResult<string>>;