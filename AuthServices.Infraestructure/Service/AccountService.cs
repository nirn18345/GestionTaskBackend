using AuthServices.Application.Exceptions;
using AuthServices.Application.Interface;
using AuthServices.Application.Model.Account;
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

namespace AuthServices.Infraestructure.Service
{
    public class AccountService:IAccountService
    {
        public readonly AuthDbContext _context;
        public readonly IConfiguration _configuration;
        private readonly ILogger<AccountService> _logger;


        public AccountService(AuthDbContext context, IConfiguration configuration, ILogger<AccountService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            
        }

        public async Task<AccountResponse> CreatedAccount(AccountRequest request)
        {
            _logger.LogInformation("{Proceso} {Descripcion}", "Account", "Created");



            if (string.IsNullOrWhiteSpace(request.Email))
                throw new AuthenticationException(ResponseMessage.EmailNull);

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new AuthenticationException(ResponseMessage.PasswordNull);

         

            var accountExist = await _context.Account
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (accountExist != null)
                throw new RequestException(ResponseMessage.AccountExist,402);

            var hashedPassword = HashHelper.ToSha256(request.Password);

            var accountID = Guid.NewGuid(); // ✔️ Correcto

            await _context.Account.AddAsync(new Account
            {
                AccountId = accountID,
                Email = request.Email,
                Password = hashedPassword,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = request.CreateBy
            });

            await _context.SaveChangesAsync();

            return new AccountResponse
            {
                AccountId = accountID
            };
        }



       
    }
}
