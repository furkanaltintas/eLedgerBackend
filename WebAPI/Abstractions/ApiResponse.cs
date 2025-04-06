using DomainResults.Common;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Abstractions;

public static class ApiResponse
{
    public static IActionResult Response<T>(this ControllerBase controller, T result)
    {
        if (result is not null) return controller.Ok(result);
        return controller.NotFound(result);
    }

    public static IActionResult NoResponse<T>(this ControllerBase controller)
    {
        return controller.NoContent();
    }

    public static IActionResult DomResponse<T>(this ControllerBase controller, IDomainResult<T> result)
    {
        if (result.IsSuccess) return controller.Ok(result);
        return controller.NotFound(result);
    }
}