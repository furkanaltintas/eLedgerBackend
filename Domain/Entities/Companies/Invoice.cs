using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities.Companies;

public class Invoice : Entity
{
    public InvoiceTypeEnum Type { get; set; } = InvoiceTypeEnum.Purchase;
    public DateOnly Date { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public decimal Amount { get; set; }
    public List<InvoiceDetail>? Details { get; set; }
}