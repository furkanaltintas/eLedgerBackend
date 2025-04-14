using Application.Features.CashRegisters.Commands;
using Application.Features.CashRegisters.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;

namespace WebAPI.Controllers;

public class CashRegistersController : ApiController
{
    public CashRegistersController(IMediator mediator) : base(mediator) { }


    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        await Send(new GetAllCashRegistersQuery());


    [HttpPost]
    public async Task<IActionResult> Create(CreateCashRegisterCommand createCashRegisterCommand) =>
        await Send(createCashRegisterCommand);

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCashRegisterCommand updateCashRegisterCommand) =>
        await Send(updateCashRegisterCommand);

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteCashRegisterCommand deleteCashRegisterCommand) =>
        await Send(deleteCashRegisterCommand);
}