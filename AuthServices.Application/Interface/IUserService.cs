using AuthServices.Application.DTOs;
using AuthServices.Application.DTOs.Users;
using AuthServices.Application.Model;
using AuthServices.Application.Model.Account;
using AuthServices.Application.Model.User;
using AuthServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.Interface
{
    public interface IUserService
    {

        public Task<UserResponse> CreatedUser(UserRequest request);

        public Task<UserResponse> UpdateUser(UpdateUserRequest request);

        Task<PageResult<UserListDto>> GetUsersAsync(ListDtos queryParams);
        Task<UserListDto> GetUserByIdAsync(Guid UserID);

        Task<bool> DeleteUserAsync(Guid userId);

    }
}
