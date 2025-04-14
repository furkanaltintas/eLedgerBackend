using DomainResults.Common;
using MediatR;

namespace Application.Features.BankDetails.Commands;

public record DeleteBankDetailCommand(Guid Id) : IRequest<IDomainResult<string>>;