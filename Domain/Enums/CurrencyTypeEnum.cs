using Ardalis.SmartEnum;

namespace Domain.Enums;

public class CurrencyTypeEnum : SmartEnum<CurrencyTypeEnum>
{
    public static readonly CurrencyTypeEnum TL = new("TL", 1);
    public static readonly CurrencyTypeEnum USD = new("USD", 2);
    public static readonly CurrencyTypeEnum EUR = new("Euro", 3);

    public CurrencyTypeEnum(string name, int value) : base(name, value) { }
}
