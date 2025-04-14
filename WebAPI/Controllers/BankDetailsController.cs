using Application.Features.BankDetails.Commands;
using Application.Features.BankDetails.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;

namespace WebAPI.Controllers;

public class BankDetailsController : ApiController
{
    public BankDetailsController(IMediator mediator) : base(mediator) { }


    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllBankDetailsQuery getAllBankDetailsQuery) =>
        await Send(getAllBankDetailsQuery);

    [HttpPost]
    public async Task<IActionResult> Create(CreateBankDetailCommand createBankDetailCommand) =>
        await Send(createBankDetailCommand);

    [HttpPut]
    public async Task<IActionResult> Update(UpdateBankDetailCommand updateBankDetailCommand) =>
        await Send(updateBankDetailCommand);

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteBankDetailCommand deleteBankDetailCommand) =>
        await Send(deleteBankDetailCommand);
}