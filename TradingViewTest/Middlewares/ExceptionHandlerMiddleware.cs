using Entites.Exceptions;
using System.Net;
using System.Text.Json;

namespace TradingViewTest.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = (int)HttpStatusCode.InternalServerError;
        var message = "Internal server error";

        switch (exception)
        {
            case IexCloudException iexCloudException:
                code = (int)iexCloudException.ErrorCode;
                message = iexCloudException.Message;
                break;
            case NotFoundException notFoundException:
                code = (int)HttpStatusCode.NotFound;
                message = notFoundException.Message;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;

        return context.Response.WriteAsync(JsonSerializer.Serialize(new { error = message }));
    }
}
