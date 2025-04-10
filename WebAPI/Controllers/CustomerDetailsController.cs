using Application.Features.CustomerDetails.GetAllCustomerDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;

namespace WebAPI.Controllers;

public class CustomerDetailsController : ApiController
{
    public CustomerDetailsController(IMediator mediator) : base(mediator) { }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCustomerDetailsQuery getAllCustomerDetailsQuery) =>
        await Send(getAllCustomerDetailsQuery);
}