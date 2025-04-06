using Ardalis.SmartEnum;

namespace Domain.Enums;

public class InvoiceTypeEnum : SmartEnum<InvoiceTypeEnum>
{
    public static readonly InvoiceTypeEnum Purchase = new("Alış Faturası", 1);
    public static readonly InvoiceTypeEnum Selling = new("Satış Faturası", 2);
    public InvoiceTypeEnum(string name, int value) : base(name, value) { }
}