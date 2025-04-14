using Application.Security;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Banks.Commands;

[Authorize("Admnin", "Manager")]
public record CreateBankCommand(
    string Name,
    string IBAN,
    int CurrencyTypeValue) : IRequest<IDomainResult<string>>;