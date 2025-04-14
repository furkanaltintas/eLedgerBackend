using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;

namespace WebAPI.Controllers;

public class ProductsController : ApiController
{
    public ProductsController(IMediator mediator) : base(mediator) { }


    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        await Send(new GetAllProductsQuery());

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand createProductCommand) =>
        await Send(createProductCommand);

    [HttpPut]
    public async Task<IActionResult> Update(UpdateProductCommand updateProductCommand) =>
        await Send(updateProductCommand);

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteProductCommand deleteProductCommand) =>
        await Send(deleteProductCommand);
}