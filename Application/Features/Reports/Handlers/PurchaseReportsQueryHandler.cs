using Application.Common.Interfaces;
using Application.Features.Reports.Constants;
using Application.Features.Reports.Queries;
using Application.Features.Reports.Responses;
using Domain.Entities.Companies;
using Domain.Enums;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Reports.Handlers;

internal sealed class PurchaseReportsQueryHandler(
    IInvoiceRepository invoiceRepository,
    ICompanyContextHelper companyContextHelper): IRequestHandler<PurchaseReportsQuery, IDomainResult<PurchaseReportsQueryResponse>>
{
    public async Task<IDomainResult<PurchaseReportsQueryResponse>> Handle(PurchaseReportsQuery request, CancellationToken cancellationToken)
    {
        PurchaseReportsQueryResponse response = companyContextHelper.GetCompanyFromContext<PurchaseReportsQueryResponse>(ReportsMessages.Cache);

        if(response is null)
        {
            List<Invoice> invoices = await invoiceRepository
                .Where(i => i.Type == InvoiceTypeEnum.Selling.Value)
                .OrderBy(i => i.Date)
                .ToListAsync(cancellationToken);

            response = new()
            {
                Dates = invoices.GroupBy(i => i.Date).Select(i => i.Key).ToList(),
                Amounts = invoices.GroupBy(i => i.Date).Select(i => i.Sum(i => i.Amount)).ToList()
            };

            companyContextHelper.SetCompanyInContext(ReportsMessages.Cache, response);
        }

        return DomainResult.Success(response);
    }
}