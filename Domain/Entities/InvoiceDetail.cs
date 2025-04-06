using Domain.Abstractions;

namespace Domain.Entities;

public class InvoiceDetail : Entity
{
    public Guid InvoiceId { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
}
