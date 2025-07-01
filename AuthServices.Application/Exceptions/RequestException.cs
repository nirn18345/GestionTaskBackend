using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.Exceptions
{
    public class RequestException : BaseCustomException
    {
        public RequestException(string? message, int statusCode = 402)
            : base(message ?? "Request Exception", statusCode)
        {
        }
    }
}
