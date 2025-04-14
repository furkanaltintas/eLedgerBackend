using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;

namespace WebAPI.Controllers;

public class UsersController : ApiController
{
    public UsersController(IMediator mediator) : base(mediator) { }


    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        await Send(new GetAllUsersQuery());

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand createUserCommand) =>
        await Send(createUserCommand);

    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserCommand updateUserCommand) =>
        await Send(updateUserCommand);

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteUserCommand deleteUserCommand) =>
        await Send(deleteUserCommand);
}