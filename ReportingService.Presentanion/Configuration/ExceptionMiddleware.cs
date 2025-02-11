using ReportingService.Application.Exceptions;
using System.Net;
using System;

namespace ReportingService.Presentanion.Configuration;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
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

    private async Task HandleEntityNotFoundExceptionAsync(HttpContext httpContext,
                                                        Exception ex)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
        await httpContext.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = httpContext.Response.StatusCode,
            Message = ex.Message
        }.ToString());
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
        await httpContext.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = httpContext.Response.StatusCode,
            Message = "Strange shit!"
        }.ToString());
    }
}
