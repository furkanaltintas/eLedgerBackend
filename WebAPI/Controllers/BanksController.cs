﻿using Application.Features.Banks.CreateBank;
using Application.Features.Banks.DeleteBank;
using Application.Features.Banks.GetAllBanks;
using Application.Features.Banks.UpdateBank;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;

namespace WebAPI.Controllers;

public class BanksController : ApiController
{
    public BanksController(IMediator mediator) : base(mediator) { }


    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        await Send(new GetAllBanksQuery());

    [HttpPost]
    public async Task<IActionResult> Create(CreateBankCommand createBankCommand) =>
        await Send(createBankCommand);

    [HttpPut]
    public async Task<IActionResult> Update(UpdateBankCommand updateBankCommand) =>
        await Send(updateBankCommand);

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteBankCommand deleteBankCommand) =>
        await Send(deleteBankCommand);
}