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
        string cacheKey = request.Type == InvoiceTypeEnum.Purchase.Value
            ? "purchaseInvoices"
            : "sellingInvoices";

        invoices = companyContextHelper.GetCompanyFromContext<List<Invoice>>(cacheKey);

        if (invoices is null)
        {
            invoices = await invoiceRepository
                .Where(i => i.Type.Value == request.Type)
                .OrderBy(i => i.Date)
                .ToListAsync(cancellationToken);

            companyContextHelper.SetCompanyInContext(cacheKey, invoices);
        }

        return DomainResult.Success(invoices);
    }
}