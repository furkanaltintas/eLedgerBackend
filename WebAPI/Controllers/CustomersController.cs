using Application.Features.Customers.CreateCustomer;
using Application.Features.Customers.DeleteCustomer;
using Application.Features.Customers.GetAllCustomers;
using Application.Features.Customers.UpdateCustomer;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;
using MediatR;

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