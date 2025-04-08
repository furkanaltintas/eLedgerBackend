using Application.Features.Auth.ChangeCompany;
using Application.Features.Auth.Login;
using Application.Features.Auth.LoginByCompany;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;

namespace WebAPI.Controllers;

public class AuthController : ApiController
{
    public AuthController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginCommand loginCommand) =>
        await Send(loginCommand);

    [AllowAnonymous]
    [HttpPost("login-by-company")]
    public async Task<IActionResult> LoginByCompany(LoginByCompanyCommand loginByCompanyCommand) =>
    await Send(loginByCompanyCommand);


    [HttpPost("change-company")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> ChangeCompany(ChangeCompanyCommand changeCompanyCommand) =>
        await Send(changeCompanyCommand);
}