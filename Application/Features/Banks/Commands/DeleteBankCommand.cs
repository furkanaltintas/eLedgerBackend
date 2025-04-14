using Application.Security;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Banks.Commands;

[Authorize("Admnin", "Manager")]
public record DeleteBankCommand(Guid Id) : IRequest<IDomainResult<string>>;