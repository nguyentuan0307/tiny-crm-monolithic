using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;
using TinyCRM.Application.Models;
using TinyCRM.Domain.Exceptions;
using TinyCRM.Infrastructure.Logger;

namespace TinyCRM.API.Middleware;

public static class CustomExceptionMiddleware
{
    public static void UseCustomerExceptionHandler(this IApplicationBuilder app, IHostEnvironment env)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                int statusCode;

                var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                LoggerService.LogError(ex, "An exception occurred while processing the request");

                var message = ex?.Message;
                switch (ex)
                {
                    case EntityNotFoundException:
                        statusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case NotImplementedException:
                        statusCode = (int)HttpStatusCode.NotImplemented;
                        break;

                    case EntityValidationException:
                    case InvalidUpdateException:
                    case InvalidPasswordException:
                        statusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    default:
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                if (!env.IsDevelopment())
                {
                    message = "An error occurred from the system. Please try again";
                }
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new ExceptionResponse
                {
                    StatusCode = statusCode,
                    Message = message
                });
            });
        });
    }
}