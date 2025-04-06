using Domain.Entities;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Companies.UserCompanies;

public record UserCompaniesQuery(string UserId) : IRequest<IDomainResult<List<Company>>>;