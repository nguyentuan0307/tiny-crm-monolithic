using TinyCRM.API.Middleware;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Model;
namespace TinyCRM.API.Middleware
{
    public class HttpExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpExceptionMiddleware> _logger;
        public HttpExceptionMiddleware(RequestDelegate next, ILogger<HttpExceptionMiddleware> logger)
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
                _logger.LogError($"{ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = ex switch
            {
                HttpException httpEx => httpEx.StatusCode,
                _ => StatusCodes.Status500InternalServerError
            };

            var errorCode = ex switch
            {
                HttpException httpEx => httpEx.ErrorCode,
                _ => "Internal Server Error"
            };

            var message = ex switch
            {
                HttpException httpEx => httpEx.Message,
                _ => ex.Message
            };
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new ExceptionRespone
            {
                StatusCode = statusCode,
                ErrorCode = errorCode,
                Message = message
            });
        }
    }
}
public static class HttpExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HttpExceptionMiddleware>();
    }
}