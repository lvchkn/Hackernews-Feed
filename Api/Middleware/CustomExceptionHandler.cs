using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Shared.Exceptions;

namespace Api.Middleware;

public class CustomExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;
    private readonly RequestDelegate _next;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private const string GenericErrorMessage = "Unhandled exception occurred while processing the request";

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
            _logger.LogError(exception, GenericErrorMessage);
            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = exception switch
        {
            EntityNotFoundException => (int) HttpStatusCode.NotFound,
            UserNotFoundException => (int) HttpStatusCode.NotFound,
            InterestNotFoundException => (int) HttpStatusCode.NotFound,
            StoryNotFoundException => (int) HttpStatusCode.NotFound,

            BadHttpRequestException => (int) HttpStatusCode.BadRequest,
            JsonException => (int) HttpStatusCode.BadRequest,
            ArgumentOutOfRangeException => (int) HttpStatusCode.BadRequest,

            EntityAlreadyExistsException => (int) HttpStatusCode.BadRequest,
            UserAlreadyExistsException => (int) HttpStatusCode.BadRequest,
            InterestAlreadyExistsException => (int) HttpStatusCode.BadRequest,
            StoryAlreadyExistsException => (int) HttpStatusCode.BadRequest,

            QueryParameterException => (int) HttpStatusCode.BadRequest,

            _ => (int) HttpStatusCode.InternalServerError
        };

        var problemDetailsFactory = httpContext.RequestServices.GetService<ProblemDetailsFactory>();

        string? instance = httpContext.Request.Path.Value ?? null;

        string detail = statusCode switch
        {
            500 => GenericErrorMessage,
            _ => exception.Message
        };

        string title = statusCode switch
        {
            400 => nameof(HttpStatusCode.BadRequest),
            404 => nameof(HttpStatusCode.NotFound),
            500 => nameof(HttpStatusCode.InternalServerError),
            // ReSharper disable once UnreachableSwitchArmDueToIntegerAnalysis
            _ => "Error"
        };

        ProblemDetails problemDetails;

        if (problemDetailsFactory is null)
        {
            problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = instance
            };
        }
        else
        {
            problemDetails = problemDetailsFactory.CreateProblemDetails(httpContext: httpContext,
                statusCode: statusCode,
                title: title,
                type: null,
                detail: detail,
                instance: instance);
        }

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, _jsonSerializerOptions));
    }
}
