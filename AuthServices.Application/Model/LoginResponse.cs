using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.Model
{
    public class LoginResponse
    {

        public Guid UserId { get; set; }

        public string Token {  get; set; }
    }
}
