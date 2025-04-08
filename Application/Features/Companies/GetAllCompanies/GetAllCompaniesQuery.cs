using Domain.Entities;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Companies.GetAllCompanies;

public record GetAllCompaniesQuery() : IRequest<IDomainResult<List<Company>>>;