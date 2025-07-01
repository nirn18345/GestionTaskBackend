using AuthServices.Api.Utils;
using AuthServices.Application.DTOs;
using AuthServices.Application.DTOs.Erros;
using AuthServices.Application.Interface;
using AuthServices.Application.Model;
using AuthServices.Application.Model.Account;
using AuthServices.Application.Utils;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthServices.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthService _AuthService;

      
        public AuthController(IAuthService authService)
        {
            _AuthService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(MsDtoResponse<LoginResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult> Login([FromBody] CredentialDto loginDto)
        {
            var result = await _AuthService.AuthenticateAsync(loginDto.Email, loginDto.Password);

            if (result == null)
                return Unauthorized(ApiResponseBuilder.Error(HttpContext, 401, ResponseMessage.InvalidCredentials));

            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.LoginSuccess));
        }



       




    }
}
