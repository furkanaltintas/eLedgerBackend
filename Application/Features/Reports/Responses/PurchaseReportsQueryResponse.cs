namespace Application.Features.Reports.Responses;

public sealed record PurchaseReportsQueryResponse
{
    public List<DateOnly> Dates { get; set; } = new();
    public List<decimal> Amounts { get; set; } = new();
}