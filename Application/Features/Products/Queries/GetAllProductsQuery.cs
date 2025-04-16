using Domain.Entities.Companies;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Products.Queries;

public sealed record GetAllProductsQuery() : IRequest<IDomainResult<List<Product>>>;