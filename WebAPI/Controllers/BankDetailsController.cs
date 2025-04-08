using Application.Features.BankDetails.CreateBankDetail;
using Application.Features.BankDetails.DeleteBankDetail;
using Application.Features.BankDetails.GetAllBankDetails;
using Application.Features.BankDetails.UpdateBankDetail;
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