using Application.Features.Reports.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;

namespace WebAPI.Controllers;

public class ReportsController : ApiController
{
    public ReportsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("product-profitability-reports")]
    public async Task<IActionResult> ProductProfitabilityReports(CancellationToken cancellationToken) =>
        await Send(new ProductProfitabilityReportsQuery(), cancellationToken);

    [HttpGet("purchase-reports")]
    public async Task<IActionResult> PurchaseReports(CancellationToken cancellationToken) =>
        await Send(new PurchaseReportsQuery(), cancellationToken);
}
