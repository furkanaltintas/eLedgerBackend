using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Invoices.DeleteInvoice;

sealed class DeleteInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    ICustomerRepository customerRepository,
    ICustomerDetailRepository customerDetailRepository,
    IProductRepository productRepository,
    IProductDetailRepository productDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<DeleteInvoiceCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        Invoice? invoice = await invoiceRepository.Where(i => i.Id == request.Id).Include(i => i.Details).FirstOrDefaultAsync(cancellationToken);
        if(invoice is null) return DomainResult.NotFound<string>("Fatura bulunamadı");

        CustomerDetail? customerDetail = await customerDetailRepository.Where(c => c.InvoiceId == request.Id).FirstOrDefaultAsync(cancellationToken);
        if(customerDetail is not null)
        {
            customerDetailRepository.Delete(customerDetail);
        }

        Customer? customer = await customerRepository.Where(c => c.Id == invoice.CustomerId).FirstOrDefaultAsync(cancellationToken);
        if(customer is not null)
        {
            customer.DepositAmount -= invoice.Type.Value == InvoiceTypeEnum.Purchase.Value ? 0 : invoice.Amount;
            customer.WithdrawalAmount -= invoice.Type.Value == InvoiceTypeEnum.Selling.Value ? 0 : invoice.Amount;
            customerRepository.Update(customer);
        }

        List<ProductDetail> productDetails = await productDetailRepository.Where(p => p.InvoiceId == invoice.Id).ToListAsync(cancellationToken);

        foreach (var detail in productDetails)
        {
            Product? product = await productRepository.GetByExpressionWithTrackingAsync(p => p.Id == detail.ProductId, cancellationToken);
            if(product is not null)
            {
                product.Deposit -= invoice.Type.Value == InvoiceTypeEnum.Purchase.Value ? invoice.Amount : 0;
                product.Withdrawal -= invoice.Type.Value == InvoiceTypeEnum.Selling.Value ? invoice.Amount : 0;
                productRepository.Update(product);
            }
        }

        productDetailRepository.DeleteRange(productDetails);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
        companyContextHelper.RemoveRangeCompanyFromContext(["purchaseInvoices", "sellingInvoices", "customers", "products"]);
        return DomainResult.Success($"{invoice.Type.Name} kaydı başarıyla silindi");
    }
}