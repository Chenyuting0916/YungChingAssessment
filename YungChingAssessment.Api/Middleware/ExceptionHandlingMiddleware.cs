using System.Net;
using System.Text.Json;

namespace YungChingAssessment.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = new ErrorResponse
        {
            Success = false,
            Message = "An unexpected error occurred."
        };

        switch (exception)
        {
            case ArgumentException ex:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = ex.Message;
                break;
            
            case KeyNotFoundException ex:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Message = ex.Message;
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError(exception, "An unexpected error occurred.");
                break;
        }

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}

public class ErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
