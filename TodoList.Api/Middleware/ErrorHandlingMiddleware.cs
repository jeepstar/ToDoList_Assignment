using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using System.Collections.Generic;

namespace TodoList.Api.Middleware
{
    public class CustomErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;

        public CustomErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = Log.ForContext<CustomErrorHandlingMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //context.Request.EnableBuffering();
                //var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                //_logger.Information("Incoming request: {RequestBody}", requestBody);
                //context.Request.Body.Position = 0;

                await _next(context);
            }
            catch (JsonException jsonEx)
            {
                await HandleJsonExceptionAsync(context, jsonEx);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                JsonException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new { message = exception.Message };
            var jsonResponse = JsonSerializer.Serialize(response);

            _logger.Error(exception, "An error occurred: {Message}", exception.Message);

            await context.Response.WriteAsync(jsonResponse);
        }

        private async Task HandleJsonExceptionAsync(HttpContext context, JsonException jsonException)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var response = new
            {
                message = "Invalid JSON format.",
                details = jsonException.Message
            };
            var jsonResponse = JsonSerializer.Serialize(response);

            _logger.Error(jsonException, "JSON deserialization error: {Message}", jsonException.Message);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
