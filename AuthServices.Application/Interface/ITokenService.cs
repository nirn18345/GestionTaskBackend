using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.Interface
{
    public interface ITokenService
    {
        string GenerateToken(string email, Guid accountId, string roleName,string name,Guid userId);
    }

}
