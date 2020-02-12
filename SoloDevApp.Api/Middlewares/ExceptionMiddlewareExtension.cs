using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;


namespace SoloDevApp.Api.Middlewares
{
    public static class ExceptionMiddlewareExtension
    {
        public static void UseExceptionHadler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(err =>
            {
                err.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(new
                    {
                        context.Response.StatusCode,
                        Content = context.Response.Body,
                        Message = "Internal Server Error.",
                        DateTime = DateTime.Now
                    }.ToString());
                });
            });
        }
    }
}