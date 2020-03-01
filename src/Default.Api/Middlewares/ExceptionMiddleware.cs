using System;
using System.Net;
using System.Threading.Tasks;
using Default.Business.Utils;
using Microsoft.AspNetCore.Http;

namespace Default.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(httpContext, ex);
            }
        }

        private static void HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Logger.WriteError(exception, context);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}