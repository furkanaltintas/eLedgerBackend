using Domain.Entities.Companies;
using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisters.Queries;

public record GetAllCashRegistersQuery() : IRequest<IDomainResult<List<CashRegister>>>;