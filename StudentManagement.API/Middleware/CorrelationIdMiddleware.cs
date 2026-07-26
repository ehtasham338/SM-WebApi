using Serilog.Context;

namespace StudentManagement.API.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            
            if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
            {
                
                correlationId = Guid.NewGuid().ToString();
            }

          
            context.Items["CorrelationId"] = correlationId;

            
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey(CorrelationIdHeader))
                    {
                        context.Response.Headers.Append(CorrelationIdHeader, correlationId);
                    }
                    return Task.CompletedTask;
                });

                
                await _next(context);
            }
        }
    }
}