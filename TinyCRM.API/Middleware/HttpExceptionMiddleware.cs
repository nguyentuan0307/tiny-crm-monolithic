using TinyCRM.API.Middleware;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Model;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

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
                context.Request.EnableBuffering();
                await _next(context);
            }
            catch (Exception ex)
            {
                var logBuilder = new StringBuilder();

                logBuilder.AppendLine($"DateTime: {DateTime.Now}");
                logBuilder.AppendLine("An exception occurred:");
                logBuilder.AppendLine($"Exception: {ex.GetType().Name}");
                logBuilder.AppendLine($"Message: {ex.Message}");

                logBuilder.AppendLine($"Request Path: {context.Request.Path}");
                logBuilder.AppendLine($"Request Method: {context.Request.Method}");
                logBuilder.AppendLine($"Request Query: {context.Request.QueryString}");
                var requestBody = await GetRequestBodyAsync(context);
                logBuilder.AppendLine($"Request Body: {requestBody}");
                logBuilder.AppendLine($"Request Scheme: {context.Request.Scheme}");
                logBuilder.AppendLine($"Request Headers: {string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}"))}");


                if (ex is HttpException)
                {
                    _logger.LogWarning(logBuilder.ToString());
                }
                else
                {
                    _logger.LogError(logBuilder.ToString());
                }

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
        private async Task<string> GetRequestBodyAsync(HttpContext context)
        {
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Request.Body);
            var requestBody = await reader.ReadToEndAsync();

            return "{" + new Regex(@"(\n |\r |\n|{|})+").Replace(requestBody, "") + " }";
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