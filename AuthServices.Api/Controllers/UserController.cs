using AuthServices.Api.Utils;
using AuthServices.Application.DTOs;
using AuthServices.Application.DTOs.Erros;
using AuthServices.Application.DTOs.Users;
using AuthServices.Application.Interface;
using AuthServices.Application.Model.Account;
using AuthServices.Application.Model.User;
using AuthServices.Application.Utils;
using AuthServices.Infraestructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthServices.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public readonly IUserService _UserService;


        public UserController(IUserService userService)
        {
            _UserService = userService;
        }


        [HttpPost("createdUser")]
        [ProducesResponseType(typeof(MsDtoResponse<UserResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult> CreatedUser([FromBody] UserRequest request)
        {
            var result = await _UserService.CreatedUser(request);

            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.UserCreated));
        }


        [HttpPut("updatedUser")]
        [ProducesResponseType(typeof(MsDtoResponse<UserResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var result = await _UserService.UpdateUser(request);

            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.UserUpdate));
        }

        [HttpGet("listUsers")]
        [ProducesResponseType(typeof(ListDtos), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<IActionResult> GetUsers([FromQuery] ListDtos queryParams)
        {
            var result = await _UserService.GetUsersAsync(queryParams);
            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.UserList));
        }



        [HttpGet("getUserById")]
        [ProducesResponseType(typeof(UserListDto), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<IActionResult> GetUsersbyID(Guid UserID)
        {
            var result = await _UserService.GetUserByIdAsync(UserID);
            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.UserFound));
        }



        [HttpDelete("deleteUser/{id}")]
        [ProducesResponseType(typeof(MsDtoResponse<string>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 404)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var result = await _UserService.DeleteUserAsync(id);

            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.UserDelete));
        }


    }
}
