using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.Exceptions
{
    public class AuthenticationException : BaseCustomException
    {
        public AuthenticationException(string? message, int statusCode = 401)
            : base(message ?? "Authentication Exception", statusCode)
        {
        }
    }
}
