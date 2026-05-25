using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AlertSystem.API.Common.Exceptions.Handler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(
           "Error Message: {exceptionMessage}, Time of occurence {time}",
           exception.Message, DateTime.UtcNow
           );

        (string Detail, string Title, int StatusCode, string Type) details = exception switch
        {
            InternalServerException ise => (
               ise.Details ?? exception.Message,
               exception.GetType().Name,
               httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError,
               "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            ),
            ValidationException => (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest,
                 "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            ),
            BadRequestException bre => (
                bre.Details ?? exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest,
                 "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            ),
            NotFoundException => (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound,
                "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4"
            ),
            _ =>
            (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError,
                "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            )
        };

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Status = details.StatusCode,
            Detail = details.Detail,
            Instance = httpContext.Request.Path,
            Type = details.Type
        };

        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
        problemDetails.Extensions.Add("timestamp", DateTime.UtcNow.ToString("O"));
        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("errors", validationException.Errors.Select(err => err.ErrorMessage));
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        return true;

    }
}