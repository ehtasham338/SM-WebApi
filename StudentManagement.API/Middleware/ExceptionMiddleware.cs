using System.Net;
using System.Text.Json;

namespace StudentManagement.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
               
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "An unhandled exception occurred."); 
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Default status code 500 (Internal Server Error)
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Internal Server Error. Please try again later.";

            // Custom exceptions handle 
            if (exception is ArgumentException || exception is ArgumentNullException)
            {
                statusCode = HttpStatusCode.BadRequest; 
                message = exception.Message;
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound; 
                message = exception.Message;
            }

            context.Response.StatusCode = (int)statusCode;

            
            string correlationId = context.Items.ContainsKey("CorrelationId")
                ? context.Items["CorrelationId"]?.ToString() ?? ""
                : "";

            
            var response = new
            {
                statusCode = (int)statusCode,
                message = message,
                correlationId = correlationId, 
                detailedError = context.Request.Host.Value.Contains("localhost") ? exception.Message : null
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}