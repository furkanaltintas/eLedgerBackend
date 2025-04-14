using MediatR;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Security.Claims;

namespace Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttribute = request.GetType().GetCustomAttribute<Application.Security.AuthorizeAttribute>();
        if(authorizeAttribute is not null)
        {
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
            if(user is null || !authorizeAttribute.Roles.Any(role => user.IsInRole(role)))
                throw new UnauthorizedAccessException("Yetkiniz yok.");
        }

        return await next();
    }
}