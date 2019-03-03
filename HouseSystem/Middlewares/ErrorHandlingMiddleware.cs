using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Monitoring.Exceptions;
using Newtonsoft.Json;

namespace HouseSystem.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            if (exception is ApplicationException || 
                exception is ArgumentException || 
                exception is NotFoundException ||
                exception is DuplicateException)
            {
                code = HttpStatusCode.BadRequest;
            } 
            else 
            {
                throw exception;
            }

            var result = JsonConvert.SerializeObject(new {
                status = (int)code,
                error = exception.Message,
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}