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
            _logger.LogError(exception.Message);
            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = exception switch
        {
            NotFoundException => (int) HttpStatusCode.NotFound,
            EntityAlreadyExistsException => (int) HttpStatusCode.BadRequest,
            ArgumentException => (int) HttpStatusCode.BadRequest,
            _ => (int) HttpStatusCode.InternalServerError
        };

        return httpContext.Response.WriteAsync(new ExceptionData
        {
            StatusCode = httpContext.Response.StatusCode,
            Message = exception.Message
        }.ToString());
    }
    
    private record ExceptionData
    {
        public int StatusCode { get; init; }
        public string Message { get; init; } = default!;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
