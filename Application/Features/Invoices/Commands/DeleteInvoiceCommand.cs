using DomainResults.Common;
using MediatR;

namespace Application.Features.Invoices.Commands;

public sealed record DeleteInvoiceCommand(Guid Id) : IRequest<IDomainResult<string>>;