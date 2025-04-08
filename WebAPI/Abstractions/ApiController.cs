using DomainResults.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Abstractions;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
public class ApiController(IMediator mediator) : ControllerBase
{
    protected IMediator Mediator => mediator;

    // Genel Çevirici
    public async Task<IActionResult> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        var response = await mediator.Send(request, cancellationToken);
        return this.Response(response);
    }

    // Result yapımız
    public async Task<IActionResult> Send<TResponse>(IRequest<IDomainResult<TResponse>> request, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(request, cancellationToken);
        return this.DomResponse(response);
    }

    // No Result
    public async Task<IActionResult> NoSend<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        await mediator.Send(request, cancellationToken);
        return this.NoResponse<TRequest>();
    }
}