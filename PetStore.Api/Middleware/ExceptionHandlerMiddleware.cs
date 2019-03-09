using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PetStore.Api.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PetStore.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _environment;

        public ExceptionHandlerMiddleware(RequestDelegate next, IHostingEnvironment environment)
        {
            _next = next ?? throw new ArgumentNullException($"{nameof(next)}");
            _environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
                throw new InvalidOperationException("The response has already started, the exception middleware will not be executed.");

            // Here we can log the actual error into the internal logging system.

            var errorMessage = _environment.IsDevelopment()
                ? exception.Message ?? "Oupss! We've got an exception without the message!"
                : "An error occurred while processing your request.";

            var statusCode = HttpStatusCode.InternalServerError;

            if (exception is CustomHttpException customHttpException)
            {
                statusCode = customHttpException.StatusCode;
            }

            var result = JsonConvert.SerializeObject(new { error = errorMessage });

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(result);
        }
    }
}
