using Application.Features.ProductDetails.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;

namespace WebAPI.Controllers;

public class ProductDetailsController : ApiController
{
    public ProductDetailsController(IMediator mediator) : base(mediator) { }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllProductDetailsQuery getAllProductDetailsQuery) =>
        await Send(getAllProductDetailsQuery);
}