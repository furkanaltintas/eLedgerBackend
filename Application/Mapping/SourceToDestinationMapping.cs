using Application.Features.CashRegisters.CreateCashRegister;
using Application.Features.CashRegisters.UpdateCashRegister;
using Domain.Entities;
using Domain.Enums;
using Mapster;

namespace Application.Mapping;

public class SourceToDestinationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCashRegisterCommand, CashRegister>()
            .Map(dest => dest.CurrencyType, src => CurrencyTypeEnum.FromValue(src.TypeValue));

        config.NewConfig<UpdateCashRegisterCommand, CashRegister>()
            .Map(dest => dest.CurrencyType, src => CurrencyTypeEnum.FromValue(src.TypeValue));
    }
}