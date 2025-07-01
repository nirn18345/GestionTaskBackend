using AuthServices.Application.DTOs.Erros;
using AuthServices.Application.DTOs;
using AuthServices.Application.Exceptions;
using AuthServices.Application.Interface;
using AuthServices.Application.Model.User;
using AuthServices.Application.Utils;
using AuthServices.Domain.Entities;
using AuthServices.Infraestructure.Data;
using AuthServices.Infraestructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthServices.Application.Model;
using AuthServices.Application.DTOs.Users;

namespace AuthServices.Infraestructure.Service
{
    public class UserService:IUserService
    {

        public readonly AuthDbContext _context;
        public readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;


        public UserService(AuthDbContext context, IConfiguration configuration, ILogger<UserService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;

        }

        public async Task<UserResponse> CreatedUser(UserRequest request)
        {
            _logger.LogInformation("{Proceso} {Descripcion}", "User", "Created");



            if (string.IsNullOrWhiteSpace(request.FirstName))
                throw new AuthenticationException(ResponseMessage.FirstNameRequired);

            if (string.IsNullOrWhiteSpace(request.LastName))
                throw new AuthenticationException(ResponseMessage.LastNameRequired);

    
            if (string.IsNullOrWhiteSpace(request.Dni))
                throw new AuthenticationException(ResponseMessage.DniRequired);



            var UserExist = await _context.Users
                .FirstOrDefaultAsync(u => u.Dni == request.Dni);

            if (UserExist != null)
                throw new RequestException(ResponseMessage.UserExist);

            

            var UserID = Guid.NewGuid(); // ✔️ Correcto

            await _context.Users.AddAsync(new User
            {
                UserId = UserID,
               FirstName = request.FirstName,
               LastName = request.LastName,
               Dni = request.Dni,
               Phone=request.Phone,
               AccountId=request.AccountId,
               RoleId=request.RoleId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = request.CreatedBy
            });

            await _context.SaveChangesAsync();

            return new UserResponse
            {
                UserID = UserID
            };
        }

        public async Task<PageResult<UserListDto>> GetUsersAsync(ListDtos queryParams)
        {
            try
            {
                var query = _context.Users
                    .Where(u => u.IsActive)
                    .Include(u => u.Role)
                    .Include(u => u.Account)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(queryParams.Search))
                {
                    var searchLower = queryParams.Search.ToLower();
                    query = query.Where(u =>
                        u.FirstName.ToLower().Contains(searchLower) ||
                        (u.LastName != null && u.LastName.ToLower().Contains(searchLower)) ||
                        u.Dni.Contains(searchLower)
                    );
                }

                if (!string.IsNullOrEmpty(queryParams.SortBy))
                {
                    query = queryParams.SortBy.ToLower() switch
                    {
                        "firstname" => queryParams.SortDescending ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
                        "createdat" => queryParams.SortDescending ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt),
                        _ => query.OrderBy(u => u.FirstName)
                    };
                }
                else
                {
                    query = query.OrderBy(u => u.FirstName);
                }

                var totalRecords = await query.CountAsync();

                var users = await query
                    .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                    .Take(queryParams.PageSize)
                    .Select(u => new UserListDto
                    {
                        UserId = u.UserId,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Dni = u.Dni,
                        Phone = u.Phone,
                        Email = u.Account.Email,
                        AccountId=u.AccountId,
                        RoleId=u.RoleId,
                        RoleName = u.Role.RoleName,
                    })
                    .ToListAsync();

                return new PageResult<UserListDto>
                {
                    TotalRecords = totalRecords,
                    PageNumber = queryParams.PageNumber,
                    PageSize = queryParams.PageSize,
                    Data = users
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar usuarios.");
                throw new ApplicationException("Error al listar usuarios.");
            }
        }




        public async Task<UserListDto> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.UserId == userId && u.IsActive)
                    .Include(u => u.Role)
                    .Include(u => u.Account)
                    .Select(u => new UserListDto
                    {
                        UserId = u.UserId,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Dni = u.Dni,
                        Phone = u.Phone,
                        Email = u.Account.Email,
                        AccountId = u.AccountId,
                        RoleId =u.RoleId,
                        RoleName = u.Role.RoleName
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                    throw new RequestException(ResponseMessage.UserNotFound);

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario por ID.");
                throw new ApplicationException("Error al obtener usuario.");
            }
        }



        public async Task<UserResponse> UpdateUser(UpdateUserRequest request)
        {
            _logger.LogInformation("{Proceso} {Descripcion}", "User", "Update");

            if (request.UserID == Guid.Empty)
                throw new AuthenticationException(ResponseMessage.UserIdRequired);

            if (string.IsNullOrWhiteSpace(request.FirstName))
                throw new AuthenticationException(ResponseMessage.FirstNameRequired);

            if (string.IsNullOrWhiteSpace(request.LastName))
                throw new AuthenticationException(ResponseMessage.LastNameRequired);

            if (string.IsNullOrWhiteSpace(request.Dni))
                throw new AuthenticationException(ResponseMessage.DniRequired);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == request.UserID);

            if (user == null)
                throw new RequestException(ResponseMessage.UserNotFound);

            // Validar si el DNI ya lo tiene otro usuario
            var dniOwner = await _context.Users
                .FirstOrDefaultAsync(u => u.Dni == request.Dni && u.UserId != request.UserID);

            if (dniOwner != null)
                throw new RequestException(ResponseMessage.DniAlreadyExists);

            // Actualizar campos
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Dni = request.Dni;
            user.Phone = request.Phone;
            user.AccountId = request.AccountId;
            user.RoleId = request.RoleId;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = request.ModifiedBy;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UserResponse
            {
                UserID = user.UserId,
            };
        }


        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);

            if (user == null)
                throw new RequestException(ResponseMessage.UserNotFound);
            // Desactivación del usuario (eliminación lógica)
            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            // Buscar la cuenta asociada y desactivarla también
            var account = await _context.Account.FirstOrDefaultAsync(a => a.AccountId == user.AccountId && a.IsActive);

            if (account != null)
            {
                account.IsActive = false;
                account.UpdatedAt= DateTime.UtcNow;
                _context.Account.Update(account);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }




    }
}
