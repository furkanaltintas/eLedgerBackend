using Domain.Abstractions;

namespace Domain.Entities.Companies;

public class Unit : Entity
{
    public string Name { get; set; } = string.Empty; // Kilogram
    public string Symbol { get; set; } = string.Empty; // kg

    public List<Product>? Products { get; set; }
}