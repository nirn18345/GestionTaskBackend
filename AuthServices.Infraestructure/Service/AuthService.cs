using AuthServices.Application.Exceptions;
using AuthServices.Application.Interface;
using AuthServices.Application.Model;
using AuthServices.Application.Utils;
using AuthServices.Domain.Entities;
using AuthServices.Infraestructure.Data;
using AuthServices.Infraestructure.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Infraestructure.Service
{
    public class AuthService : IAuthService
    {
        public readonly AuthDbContext _context;
        public readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;



        public AuthService(AuthDbContext context, IConfiguration configuration, ILogger<AuthService> logger, ITokenService tokenService)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _tokenService = tokenService;
        }
        public async Task<LoginResponse> AuthenticateAsync(string email, string password)
        {
            _logger.LogInformation("{Proceso} {Descripcion}", "Login", "Sign In");
            
            
            if (string.IsNullOrWhiteSpace(email))
                throw new AuthenticationException(ResponseMessage.EmailNull);

            if (string.IsNullOrWhiteSpace(password))
                throw new AuthenticationException(ResponseMessage.PasswordNull);


            var hashedPassword = HashHelper.ToSha256(password);

            var Account = await _context.Account
                    .Include(a => a.User)                  
                    .ThenInclude(u => u.Role)              
                    .FirstOrDefaultAsync(a => a.Email == email && a.Password == hashedPassword && a.IsActive);

            if (Account == null)
                throw new AuthenticationException(ResponseMessage.InvalidCredentials);

            var result = await (from acc in _context.Account
                                join user in _context.Users on acc.AccountId equals user.AccountId
                                join role in _context.Role on user.RoleId equals role.RoleId
                                where acc.Email == email && acc.Password == hashedPassword
                                select new
                                {
                                    acc.AccountId,
                                    acc.Email,
                                    UserId=user.UserId,
                                    Name=user.FirstName + " "+ user.LastName,
                                    RoleName = role.RoleName,
                                    RoleId = role.RoleId
                                }).FirstOrDefaultAsync();

            if (result == null)
                throw new AuthenticationException(ResponseMessage.InvalidCredentials);

           

            var token = _tokenService.GenerateToken(Account.Email, Account.AccountId,result.RoleName,result.Name,result.UserId);

            return new LoginResponse
            {
                UserId = Account.AccountId,
                Token = token
            };

        }

        






        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }

    }
}
