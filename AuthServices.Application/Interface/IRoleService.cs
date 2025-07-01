using AuthServices.Application.DTOs;
using AuthServices.Application.Model;
using AuthServices.Application.Model.Role;
using AuthServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.Interface
{
    public interface IRoleService
    {

        Task<RolResponse> CreatedRole(RoleRequest request);


        Task<PageResult<Role>> GetRolesAsync(ListDtos queryParams);
    }
}
