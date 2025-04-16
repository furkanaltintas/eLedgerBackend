using Application.Common.Interfaces;
using Application.Features.Invoices.Commands;
using Application.Features.Invoices.Constants;
using Domain.Entities.Companies;
using Domain.Enums;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Invoices.Handlers;

sealed class DeleteInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    ICustomerRepository customerRepository,
    ICustomerDetailRepository customerDetailRepository,
    IProductRepository productRepository,
    IProductDetailRepository productDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper,
    ISignalRService signalRService) : IRequestHandler<DeleteInvoiceCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        Invoice? invoice = await invoiceRepository.Where(i => i.Id == request.Id).Include(i => i.Details).FirstOrDefaultAsync(cancellationToken);
        if (invoice is null) return DomainResult.NotFound<string>(InvoicesMessages.NotFound);

        CustomerDetail? customerDetail = await customerDetailRepository.Where(c => c.InvoiceId == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerDetail is not null) customerDetailRepository.Delete(customerDetail);

        Customer? customer = await customerRepository.Where(c => c.Id == invoice.CustomerId).FirstOrDefaultAsync(cancellationToken);
        if (customer is not null)
        {
            customer.DepositAmount -= invoice.Type.Value == InvoiceTypeEnum.Purchase.Value ? 0 : invoice.Amount;
            customer.WithdrawalAmount -= invoice.Type.Value == InvoiceTypeEnum.Selling.Value ? 0 : invoice.Amount;
            customerRepository.Update(customer);
        }

        List<ProductDetail> productDetails = await productDetailRepository.Where(p => p.InvoiceId == invoice.Id).ToListAsync(cancellationToken);

        foreach (var detail in productDetails)
        {
            Product? product = await productRepository.GetByExpressionWithTrackingAsync(p => p.Id == detail.ProductId, cancellationToken);
            if (product is not null)
            {
                product.Deposit -= detail.Deposit;
                product.Withdrawal -= detail.Withdrawal;
                productRepository.Update(product);
            }
        }

        invoiceRepository.Delete(invoice);
        productDetailRepository.DeleteRange(productDetails);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
        companyContextHelper.RemoveRangeCompanyFromContext(["invoices", "customers", "products", "purchase_reports"]);

        if (invoice.Type == InvoiceTypeEnum.Selling) await signalRService.SendDeleteReportAsync(new { Date = invoice.Date, Amount = invoice.Amount });

        return DomainResult.Success($"{invoice.Type.Name} kaydı başarıyla silindi");
    }
}