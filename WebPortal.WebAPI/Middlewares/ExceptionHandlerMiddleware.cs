using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Serilog;
using WebPortal.Application.Exceptions;
using WebPortal.Persistence.Exceptions;

namespace WebPortal.WebAPI.Middlewares;

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
            await _next.Invoke(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;
        switch (exception)
        {
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(validationException);
                break;
            case NotFoundException notFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case UserAccessDeniedExceptions userAccessDeniedExceptions:
                code = HttpStatusCode.Forbidden;
                break;
            case ArgumentException argumentException:
                code = HttpStatusCode.BadRequest;
                break;
            case IOException ioException:
                code = HttpStatusCode.BadRequest;
                Log.Error("ExceptionHandlerMiddleware {@message}", ioException.Message);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        Log.Warning("ExceptionHandlerMiddleware {@code} {@message}", (int)code, exception.Message);
        if (result == string.Empty)
        {
            result = JsonSerializer.Serialize(new {error = exception.Message});
        }

        return context.Response.WriteAsync(result);
    }
}