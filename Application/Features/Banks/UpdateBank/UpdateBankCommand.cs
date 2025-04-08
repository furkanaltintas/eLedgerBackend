using DomainResults.Common;
using MediatR;

namespace Application.Features.Banks.UpdateBank;

public record UpdateBankCommand(
    Guid Id,
    string Name,
    string IBAN,
    int CurrencyTypeValue) : IRequest<IDomainResult<string>>;