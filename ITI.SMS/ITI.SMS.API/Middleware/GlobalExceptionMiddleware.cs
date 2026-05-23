using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ITI.SMS.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred during request execution.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var statusCode = HttpStatusCode.InternalServerError;
        var errorCode = "INTERNAL_SERVER_ERROR";
        var details = new[] { exception.Message };

        if (exception is ITI.SMS.Application.Common.Exceptions.InvalidCredentialsException)
        {
            statusCode = HttpStatusCode.Unauthorized;
            errorCode = "INVALID_CREDENTIALS";
            details = new[] { "Email or password is incorrect." };
        }
        else if (exception is ITI.SMS.Application.Common.Exceptions.ValidationException)
        {
            statusCode = HttpStatusCode.BadRequest;
            errorCode = "VALIDATION_ERROR";
            details = new[] { exception.Message };
        }
        else if (exception is ITI.SMS.Application.Common.Exceptions.NotFoundException)
        {
            statusCode = HttpStatusCode.NotFound;
            errorCode = "NOT_FOUND";
            details = new[] { exception.Message };
        }
        else if (exception is ITI.SMS.Application.Common.Exceptions.ForbiddenException)
        {
            statusCode = HttpStatusCode.Forbidden;
            errorCode = "FORBIDDEN";
            details = new[] { exception.Message };
        }

        context.Response.StatusCode = (int)statusCode;

        var responseEnvelope = new
        {
            success = false,
            error = new
            {
                code = errorCode,
                details = details
            }
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonResponse = JsonSerializer.Serialize(responseEnvelope, options);
        return context.Response.WriteAsync(jsonResponse);
    }
}
