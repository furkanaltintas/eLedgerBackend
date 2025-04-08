using Application.Features.CashRegisters.CreateCashRegister;
using Application.Features.CashRegisters.DeleteCashRegister;
using Application.Features.CashRegisters.GetAllCashRegisters;
using Application.Features.CashRegisters.UpdateCashRegister;
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