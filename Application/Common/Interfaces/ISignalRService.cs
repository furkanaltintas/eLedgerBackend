namespace Application.Common.Interfaces;

public interface ISignalRService
{
    Task SendPurchaseReportAsync(object data);
    Task SendDeleteReportAsync(object data);
}