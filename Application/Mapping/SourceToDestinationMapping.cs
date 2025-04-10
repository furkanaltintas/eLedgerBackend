using Application.Features.Banks.CreateBank;
using Application.Features.Banks.UpdateBank;
using Application.Features.CashRegisters.CreateCashRegister;
using Application.Features.CashRegisters.UpdateCashRegister;
using Application.Features.Customers.CreateCustomer;
using Application.Features.Customers.UpdateCustomer;
using Domain.Entities;
using Domain.Enums;
using Mapster;

namespace Application.Mapping;

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
    }
}