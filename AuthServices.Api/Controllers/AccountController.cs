using AuthServices.Api.Utils;
using AuthServices.Application.DTOs.Erros;
using AuthServices.Application.Interface;
using AuthServices.Application.Model.Account;
using AuthServices.Application.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthServices.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        public readonly IAccountService _AccountService;


        public AccountController(IAccountService accountService)
        {
            _AccountService = accountService;
        }

        
        [HttpPost("createdAccount")]
        [ProducesResponseType(typeof(MsDtoResponse<AccountResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 402)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult> CreatedAccount([FromBody] AccountRequest request)
        {
            var result = await _AccountService.CreatedAccount(request);

            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.AccountCreated));
        }
    }
}
