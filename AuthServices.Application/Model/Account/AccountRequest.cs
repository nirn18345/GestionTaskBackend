using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.Model.Account
{
    public class AccountRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CreateBy { get; set; }
    }
}
