using Application.Features.CashRegisterDetails.Commands;
using Application.Features.CashRegisterDetails.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;

namespace WebAPI.Controllers;

public class CashRegisterDetailsController : ApiController
{
    public CashRegisterDetailsController(IMediator mediator) : base(mediator) { }


    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCashRegisterDetailsQuery getAllCashRegisterDetailsQuery) =>
        await Send(getAllCashRegisterDetailsQuery);


    [HttpPost]
    public async Task<IActionResult> Create(CreateCashRegisterDetailCommand createCashRegisterDetailCommand) =>
        await Send(createCashRegisterDetailCommand);

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCashRegisterDetailCommand updateCashRegisterDetailCommand) =>
        await Send(updateCashRegisterDetailCommand);

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteCashRegisterDetailCommand deleteCashRegisterDetailCommand) =>
        await Send(deleteCashRegisterDetailCommand);
}