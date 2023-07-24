using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models;

namespace TinyCRM.API.Middleware
{
    public static class CustomExceptionMiddleware
    {
        public static void UseCustomerExceptionHandler(this IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    Log.Error(ex, "An exception occurred while processing the request");

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
                        _ => ex?.Message
                    };

                    if (!env.IsDevelopment())
                    {
                        message = "An error occurred from the system. Please try again";
                    }
                    context.Response.StatusCode = statusCode;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new ExceptionRespone
                    {
                        StatusCode = statusCode,
                        ErrorCode = errorCode,
                        Message = message
                    });
                });
            });
        }
    }
}