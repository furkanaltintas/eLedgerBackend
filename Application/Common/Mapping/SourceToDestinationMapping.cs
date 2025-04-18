using Application.Features.Banks.Commands;
using Application.Features.CashRegisters.Commands;
using Application.Features.Customers.Commands;
using Application.Features.Invoices.Commands;
using Domain.Dtos;
using Domain.Entities.Companies;
using Domain.Enums;
using Mapster;

namespace Application.Common.Mapping;

public class SourceToDestinationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCashRegisterCommand, CashRegister>()
            .Map(dest => dest.CurrencyType, src => CurrencyTypeEnum.FromValue(src.CurrencyTypeValue));

        config.NewConfig<UpdateCashRegisterCommand, CashRegister>()
            .Map(dest => dest.CurrencyType, src => CurrencyTypeEnum.FromValue(src.CurrencyTypeValue));

        config.NewConfig<CreateBankCommand, Bank>()
            .Map(dest => dest.CurrencyType, src => CurrencyTypeEnum.FromValue(src.CurrencyTypeValue));

        config.NewConfig<UpdateBankCommand, Bank>()
            .Map(dest => dest.CurrencyType, src => CurrencyTypeEnum.FromValue(src.CurrencyTypeValue));


        config.NewConfig<CreateCustomerCommand, Customer>()
            .Map(dest => dest.Type, src => CustomerTypeEnum.FromValue(src.TypeValue));

        config.NewConfig<UpdateCustomerCommand, Customer>()
            .Map(dest => dest.Type, src => CustomerTypeEnum.FromValue(src.TypeValue));

        config.NewConfig<CreateInvoiceCommand, Invoice>()
            .Map(dest => dest.Type, src => InvoiceTypeEnum.FromValue(src.TypeValue))
            .Map(dest => dest.Amount, src => src.Details.Sum(id => id.Quantity * id.Price))
            .Map(dest => dest.Details, src => src.Details.Select(id => new InvoiceDetailDto(id.ProductId, id.Quantity, id.Price)));
    }
}