using AuthServices.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace AuthServices.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // continua normalmente
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex, context.TraceIdentifier);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, string traceId)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            int errorCode;
            string message;
            List<string> errorsList = new List<string>();

            if (exception is BaseCustomException customEx)
            {
                statusCode = customEx.Code;
                errorCode = customEx.Code;
                message = customEx.Message;

                // Aquí agregamos el mensaje personalizado de la excepción
                errorsList.Add(customEx.Message);
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                errorCode = 500;
                message = "An unexpected error occurred.";

                // Agregamos mensaje genérico si es un error inesperado
                errorsList.Add(exception.Message);
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                code = errorCode,
                traceid = traceId,
                message = message,
                errors = errorsList.Select(error => new
                {
                    code = errorCode,
                    message = error
                })
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }

       




    }
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            return app;
        }
    }
}
