using Domain.Abstractions;

namespace Domain.Entities.Companies;

public class Product : Entity
{
    public Guid CategoryId { get; set; }
    public Guid UnitId { get; set; }

    public string Name { get; set; } = string.Empty;
    public decimal Deposit { get; set; }
    public decimal Withdrawal { get; set; }

    public List<ProductDetail>? Details { get; set; }
}