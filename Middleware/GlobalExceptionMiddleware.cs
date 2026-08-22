using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace IPL.ECommerce.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception occurred.");

            await HandleExceptionAsync(
                context,
                ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentException =>
                HttpStatusCode.BadRequest,

            KeyNotFoundException =>
                HttpStatusCode.NotFound,

            InvalidOperationException =>
                HttpStatusCode.BadRequest,

            UnauthorizedAccessException =>
                HttpStatusCode.Unauthorized,

            _ =>
                HttpStatusCode.InternalServerError
        };

        var problem = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = statusCode switch
            {
                HttpStatusCode.BadRequest =>
                    "Bad Request",

                HttpStatusCode.NotFound =>
                    "Resource Not Found",

                HttpStatusCode.Unauthorized =>
                    "Unauthorized",

                _ =>
                    "An unexpected error occurred."
            },

            Detail =
                statusCode ==
                HttpStatusCode.InternalServerError
                    ? "An unexpected error occurred."
                    : exception.Message,

            Instance = context.Request.Path
        };

        context.Response.StatusCode =
            (int)statusCode;

        context.Response.ContentType =
            "application/problem+json";

        await context.Response.WriteAsJsonAsync(problem);
    }
}