using Domain.Entities;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Banks.GetAllBanks;

public record GetAllBanksQuery() : IRequest<IDomainResult<List<Bank>>>;