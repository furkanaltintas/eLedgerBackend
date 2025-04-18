using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities.Companies;

public class CashRegister : Entity
{
    public string Name { get; set; } = string.Empty;
    public CurrencyTypeEnum CurrencyType { get; set; } = CurrencyTypeEnum.TL;
    public decimal DepositAmount { get; set; } //Giriş
    public decimal WithdrawalAmount { get; set; } //Çıkış
    public List<CashRegisterDetail>? Details { get; set; }
}