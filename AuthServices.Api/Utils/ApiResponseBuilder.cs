using AuthServices.Application.DTOs.Erros;

namespace AuthServices.Api.Utils
{
    public static class ApiResponseBuilder
    {
        public static MsDtoResponse<T> Success<T>(T data, HttpContext context, string message = "Operación exitosa", int code = 200)
        {
            return new MsDtoResponse<T>(data, context.TraceIdentifier, code, message);
        }

        public static MsDtoResponseError Error(HttpContext context, int code, string message, IEnumerable<string> errors = null)
        {
            return new MsDtoResponseError(code, message, context.TraceIdentifier, errors ?? new[] { message });
        }
    }

}
