using AuthServices.Application.DTOs;
using AuthServices.Application.Exceptions;
using AuthServices.Application.Interface;
using AuthServices.Application.Model;
using AuthServices.Application.Model.Role;
using AuthServices.Application.Model.User;
using AuthServices.Application.Utils;
using AuthServices.Domain.Entities;
using AuthServices.Infraestructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Infraestructure.Service
{
    public class RoleService: IRoleService
    {
        public readonly AuthDbContext _context;
        public readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RoleService> _logger;


        public RoleService(AuthDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<RoleService> logger)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }



        public async Task<RolResponse> CreatedRole(RoleRequest request)
        {
            _logger.LogInformation("{Proceso} {Descripcion}", "Rol", "Created");



            if (string.IsNullOrWhiteSpace(request.RoleName))
                throw new AuthenticationException(ResponseMessage.RolRequired);





            var RolExist = await _context.Role
                .FirstOrDefaultAsync(u => u.RoleName == request.RoleName);

            if (RolExist != null)
                throw new RequestException(ResponseMessage.RolExist);



            var RolID = Guid.NewGuid(); 

            await _context.Role.AddAsync(new Role
            {
               RoleId= RolID,
               RoleName=request.RoleName,
               IsActive=true,
               CreatedAt=DateTime.Now,
               CreatedBy=request.CreatedBy 
            });

            await _context.SaveChangesAsync();

            return new RolResponse
            {
                RoleId = RolID
            };
        }


        public async Task<PageResult<Role>> GetRolesAsync(ListDtos queryParams)
        {
            try
            {
                var query = _context.Role
                    .Where(u => u.IsActive)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(queryParams.Search))
                {
                    var searchLower = queryParams.Search.ToLower();
                    query = query.Where(u =>
                        u.RoleName.ToLower().Contains(searchLower) 
                    );
                }

                if (!string.IsNullOrEmpty(queryParams.SortBy))
                {
                    query = queryParams.SortBy.ToLower() switch
                    {
                        "roleName" => queryParams.SortDescending ? query.OrderByDescending(u => u.RoleName) : query.OrderBy(u => u.RoleName),
                        "createdat" => queryParams.SortDescending ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt),
                        _ => query.OrderBy(u => u.RoleName)
                    };
                }
                else
                {
                    query = query.OrderBy(u => u.RoleName);
                }

                var totalRecords = await query.CountAsync();

                var roles = await query
                    .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                    .Take(queryParams.PageSize)
                    .ToListAsync();

                return new PageResult<Role>
                {
                    TotalRecords = totalRecords,
                    PageNumber = queryParams.PageNumber,
                    PageSize = queryParams.PageSize,
                    Data = roles
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar usuarios.");
                throw new ApplicationException("Error al listar usuarios.");
            }
        }

    }
}
