using AuthServices.Application.Model;
using AuthServices.Application.Model.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.Interface
{
    public interface IAuthService
    {
        Task<LoginResponse> AuthenticateAsync(string Email, string Password);

   


    }
}
