using Domain.Dtos;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Invoices.Commands;

public sealed record CreateInvoiceCommand(
    int TypeValue,
    DateOnly Date,
    string InvoiceNumber,
    Guid CustomerId,
    List<InvoiceDetailDto> Details) : IRequest<IDomainResult<string>>;