using Application.Common.Handlers.Companies;
using Application.Common.Interfaces;
using Application.Features.Invoices.Constants;
using Application.Features.Invoices.Queries;
using Domain.Entities.Companies;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Invoices.Handlers;

internal sealed class GetAllInvoicesQueryHandler: CompanyCacheableQueryHandlerBase, IRequestHandler<GetAllInvoicesQuery, IDomainResult<List<Invoice>>>
{
    private readonly IInvoiceRepository _invoiceRepository;

    public GetAllInvoicesQueryHandler(ICompanyContextHelper companyContextHelper, IInvoiceRepository invoiceRepository) : base(companyContextHelper)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<IDomainResult<List<Invoice>>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
    {
        return await Success(InvoicesMessages.Cache, async () =>
        {
            return await _invoiceRepository.GetAll()
                                    .Include(i => i.Customer)
                                    .Include(i => i.Details!)
                                    .ThenInclude(i => i.Product)
                                    .OrderBy(i => i.Date)
                                    .ToListAsync(cancellationToken);    
        });
    }
}