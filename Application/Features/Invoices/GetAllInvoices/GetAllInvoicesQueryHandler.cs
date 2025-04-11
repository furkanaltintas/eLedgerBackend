using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Invoices.GetAllInvoices;

sealed class GetAllInvoicesQueryHandler(
    IInvoiceRepository invoiceRepository,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<GetAllInvoicesQuery, IDomainResult<List<Invoice>>>
{
    public async Task<IDomainResult<List<Invoice>>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
    {
        List<Invoice>? invoices;

        invoices = companyContextHelper.GetCompanyFromContext<List<Invoice>>("invoices");

        if (invoices is null)
        {
            invoices = await invoiceRepository
                .GetAll()
                .Include(i => i.Customer)
                .Include(i => i.Details!)
                .ThenInclude(i => i.Product)
                .OrderBy(i => i.Date)
                .ToListAsync(cancellationToken);

            companyContextHelper.SetCompanyInContext("invoices", invoices);
        }

        return DomainResult.Success(invoices);
    }
}