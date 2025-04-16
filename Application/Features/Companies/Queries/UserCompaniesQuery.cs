using Domain.Entities.Partners;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Companies.Queries;

public sealed record UserCompaniesQuery(Guid UserId) : IRequest<IDomainResult<List<Company>>>;