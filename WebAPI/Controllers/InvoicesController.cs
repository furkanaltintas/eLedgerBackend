using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;
using MediatR;
using Application.Features.Invoices.Queries;
using Application.Features.Invoices.Commands;

namespace WebAPI.Controllers;

public class InvoicesController : ApiController
{
    public InvoicesController(IMediator mediator) : base(mediator) { }


    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllInvoicesQuery getAllInvoicesQuery) =>
        await Send(getAllInvoicesQuery);

    [HttpPost]
    public async Task<IActionResult> Create(CreateInvoiceCommand createInvoiceCommand) =>
        await Send(createInvoiceCommand);

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteInvoiceCommand deleteInvoiceCommand) =>
        await Send(deleteInvoiceCommand);
}