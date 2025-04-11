using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Invoices.CreateInvoice;

sealed class CreateInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IProductRepository productRepository,
    IProductDetailRepository productDetailRepository,
    ICustomerRepository customerRepository,
    ICustomerDetailRepository customerDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper,
    IMapper mapper) : IRequestHandler<CreateInvoiceCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        #region Fatura ve Detay kısmı
        Invoice invoice = mapper.Map<Invoice>(request);
        await invoiceRepository.AddAsync(invoice, cancellationToken);
        #endregion

        #region Customer ve Detay kısmı
        Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.CustomerId, cancellationToken);
        if (customer is null) return DomainResult.Failed<string>("Müşteri bulunamadı");

        customer.DepositAmount += request.TypeValue == 2 ? invoice.Amount : 0;
        customer.WithdrawalAmount += request.TypeValue == 1 ? invoice.Amount : 0;


        CustomerDetail customerDetail = new()
        {
            CustomerId = customer.Id,
            Date = request.Date,
            DepositAmount = request.TypeValue == 2 ? invoice.Amount : 0,
            WithdrawalAmount = request.TypeValue == 1 ? invoice.Amount : 0,
            Description = $"{invoice.InvoiceNumber} Numaralı {invoice.Type.Name}",
            Type = request.TypeValue == 1 ? CustomerDetailTypeEnum.PurchaseInvoice : CustomerDetailTypeEnum.SellingInvoice,
            InvoiceId = invoice.Id
        };
        await customerDetailRepository.AddAsync(customerDetail, cancellationToken);
        #endregion

        #region Product
        foreach (var item in request.Details)
        {
            Product product = await productRepository.GetByExpressionWithTrackingAsync(p => p.Id == item.ProductId, cancellationToken);
            product.Deposit += request.TypeValue == 1 ? item.Quantity : 0;
            product.Withdrawal += request.TypeValue == 2 ? item.Quantity : 0;

            ProductDetail productDetail = new()
            {
                ProductId = product.Id,
                Date = request.Date,
                Description = $"{invoice.InvoiceNumber} Numaralı {invoice.Type.Name}",
                Deposit = request.TypeValue == 1 ? item.Quantity : 0,
                Withdrawal = request.TypeValue == 2 ? item.Quantity : 0,
                InvoiceId = invoice.Id,
                Price  = item.Price
            };

            await productDetailRepository.AddAsync(productDetail, cancellationToken);
        }
        #endregion

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
        companyContextHelper.RemoveRangeCompanyFromContext(["invoices", "customers", "products"]);
        return DomainResult.Success($"{invoice.Type.Name} kaydı başarıyla tamamlandı");
    }
}