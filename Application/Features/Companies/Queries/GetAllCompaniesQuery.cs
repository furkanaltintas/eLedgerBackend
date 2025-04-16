using Domain.Entities.Partners;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Companies.Queries;

public sealed record GetAllCompaniesQuery() : IRequest<IDomainResult<List<Company>>>;