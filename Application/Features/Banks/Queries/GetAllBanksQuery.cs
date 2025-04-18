using Domain.Entities.Companies;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Banks.Queries;

public record GetAllBanksQuery() : IRequest<IDomainResult<List<Bank>>>;