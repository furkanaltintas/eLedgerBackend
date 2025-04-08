using DomainResults.Common;
using MediatR;

namespace Application.Features.BankDetails.DeleteBankDetail;

public record DeleteBankDetailCommand(Guid Id) : IRequest<IDomainResult<string>>;