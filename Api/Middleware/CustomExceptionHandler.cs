using System.Net;
using System.Text.Json;
using Shared.Exceptions;

namespace Api.Middleware;

public class CustomExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;
    private readonly RequestDelegate _next;

    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next.Invoke(httpContext);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.ToString());
            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = exception switch
        {
            NotFoundException => (int) HttpStatusCode.NotFound,
            EntityAlreadyExistsException => (int) HttpStatusCode.BadRequest,
            QueryParameterException => (int) HttpStatusCode.BadRequest,
            _ => (int) HttpStatusCode.InternalServerError
        };

        var exceptionData = new
        {
            StatusCode = httpContext.Response.StatusCode,
            Message = "Error occurred while processing the request",
        };
        
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(exceptionData));
    }
}
