using SmartphoneStore.Model.Exception;

namespace SmartphoneStore.Api.Exception;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (System.Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext httpContext, System.Exception exception)
    {
        var exceptionResult = string.Empty;
        httpContext.Response.ContentType = "application/json";

        switch (exception)
        {
            case EntityNotFoundException entityNotFound:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                break;
            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        return httpContext.Response.WriteAsync(exceptionResult);
    }
}