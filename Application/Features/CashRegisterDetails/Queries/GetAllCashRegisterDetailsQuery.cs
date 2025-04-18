using Domain.Entities.Companies;
using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisterDetails.Queries;

public record GetAllCashRegisterDetailsQuery(
    Guid CashRegisterId,
    DateOnly StartDate,
    DateOnly EndDate) : IRequest<IDomainResult<CashRegister>>;