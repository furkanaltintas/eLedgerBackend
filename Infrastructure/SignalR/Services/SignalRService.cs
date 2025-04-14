using Application.Common.Interfaces;
using Infrastructure.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.SignalR.Services;

public class SignalRService(IHubContext<ReportHub> hubContext) : ISignalRService
{
    public async Task SendPurchaseReportAsync(object data) =>
        await hubContext.Clients.All.SendAsync("PurchaseReports", data);

    public async Task SendDeleteReportAsync(object data) =>
        await hubContext.Clients.All.SendAsync("PurchaseDeleteReport", data);
}
