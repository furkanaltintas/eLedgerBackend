using Domain.Entities;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Companies.Queries;

public record UserCompaniesQuery(Guid UserId) : IRequest<IDomainResult<List<Company>>>;