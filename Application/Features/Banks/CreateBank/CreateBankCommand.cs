using DomainResults.Common;
using MediatR;

namespace Application.Features.Banks.CreateBank;

public record CreateBankCommand(
    string Name,
    string IBAN,
    int CurrencyTypeValue) : IRequest<IDomainResult<string>>;