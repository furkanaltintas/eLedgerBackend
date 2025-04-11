using Domain.Dtos;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Invoices.CreateInvoice;

public sealed record CreateInvoiceCommand(
    int TypeValue,
    DateOnly Date,
    string InvoiceNumber,
    Guid CustomerId,
    List<InvoiceDetailDto> Details) : IRequest<IDomainResult<string>>;