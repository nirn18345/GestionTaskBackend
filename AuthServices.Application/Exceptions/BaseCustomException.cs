using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.Exceptions
{
    public class BaseCustomException : Exception
    {
        public int Code { get; }

        public BaseCustomException(string message, int code) : base(message)
        {
            Code = code;
        }
    }

}
