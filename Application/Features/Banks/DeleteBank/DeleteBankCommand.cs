using DomainResults.Common;
using MediatR;

namespace Application.Features.Banks.DeleteBank;

public record DeleteBankCommand(Guid Id) : IRequest<IDomainResult<string>>;