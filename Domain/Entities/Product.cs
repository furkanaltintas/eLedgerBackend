using Domain.Abstractions;

namespace Domain.Entities;

public class Product : Entity
{
    public string Name { get; set; } = string.Empty;
    public decimal Deposit { get; set; }
    public decimal Withdrawal { get; set; }
    public List<ProductDetail>? Details { get; set; }
}