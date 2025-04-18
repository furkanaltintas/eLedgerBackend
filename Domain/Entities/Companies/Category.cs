using Domain.Abstractions;

namespace Domain.Entities.Companies;

public class Category : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<Product>? Products { get; set; }
}