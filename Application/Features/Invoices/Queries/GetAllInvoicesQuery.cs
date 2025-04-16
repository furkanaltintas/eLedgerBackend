using Domain.Entities.Companies;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Invoices.Queries;

public sealed record GetAllInvoicesQuery() : IRequest<IDomainResult<List<Invoice>>>;