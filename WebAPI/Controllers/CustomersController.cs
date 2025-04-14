using Application.Features.Customers.Queries;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;
using MediatR;
using Application.Features.Customers.Commands;

namespace WebAPI.Controllers;

public class CustomersController : ApiController
{
    public CustomersController(IMediator mediator) : base(mediator) { }


    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        await Send(new GetAllCustomersQuery());

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerCommand createCustomerCommand) =>
        await Send(createCustomerCommand);

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCustomerCommand updateCustomerCommand) =>
        await Send(updateCustomerCommand);

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteCustomerCommand deleteCustomerCommand) =>
        await Send(deleteCustomerCommand);
}