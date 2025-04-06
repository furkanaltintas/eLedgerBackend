using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class Bank  : Entity
{
    public string Name { get; set; } = string.Empty;
    public string IBAN { get; set; } = string.Empty;
    public CurrencyTypeEnum CurrencyType { get; set; } = CurrencyTypeEnum.TL;
    public decimal DepositAmount { get; set; } //Giriş
    public decimal WithdrawalAmount { get; set; } //Çıkış
    public List<BankDetail>? Details { get; set; }
}
