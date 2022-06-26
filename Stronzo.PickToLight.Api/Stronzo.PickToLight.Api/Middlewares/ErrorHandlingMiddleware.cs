using System.Net;
using FluentValidation;
using Stronzo.PickToLight.Api.Shared.Constants;
using Stronzo.PickToLight.Api.Shared.Models;

namespace Stronzo.PickToLight.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogger<ErrorHandlingMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ErrorHandlingMiddleware> logger)
    {
        var (code, errorMessage, errors) = exception switch
        {
            ValidationException ex => (HttpStatusCode.BadRequest, ex.Message, ex.Errors.Select(error => new ErrorMessage(error.ErrorCode, error.ErrorMessage)).ToArray()),
            _ => (HttpStatusCode.InternalServerError, exception.Message, new ErrorMessage[] { new(ErrorCodes.UnhandledError, "Internal server error") })
        };

        if (errorMessage is not null)
            logger.LogError("{@ErrorMessage} - {@Errors}", errorMessage, errors);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsJsonAsync(errors);
    }
}

