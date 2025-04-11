using DomainResults.Common;
using MediatR;

namespace Application.Features.Invoices.DeleteInvoice;

public sealed record DeleteInvoiceCommand(Guid Id) : IRequest<IDomainResult<string>>;