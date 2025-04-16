using Domain.Entities.Companies;
using DomainResults.Common;
using MediatR;

namespace Application.Features.ProductDetails.Queries;

public sealed record GetAllProductDetailsQuery(Guid ProductId): IRequest<IDomainResult<Product>>;