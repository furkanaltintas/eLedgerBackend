using Domain.Entities;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Products.GetAllProducts;

public sealed record GetAllProductsQuery() : IRequest<IDomainResult<List<Product>>>;