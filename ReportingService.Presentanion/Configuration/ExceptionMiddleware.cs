using ReportingService.Application.Exceptions;
using System.Net;

namespace ReportingService.Presentanion.Configuration;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (EntityNotFoundException ex)
        {
            await HandleEntityNotFoundExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task WriteErrorDetailsAsync(HttpContext httpContext, string message)
    {
        _logger.LogError($"ERROR {message}");
        await httpContext.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = httpContext.Response.StatusCode,
            Message = message,
        }.ToString());
    }

    private async Task HandleEntityNotFoundExceptionAsync(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
        await WriteErrorDetailsAsync(httpContext, ex.Message);
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await WriteErrorDetailsAsync(httpContext, "Strange shit!");
    }
}
