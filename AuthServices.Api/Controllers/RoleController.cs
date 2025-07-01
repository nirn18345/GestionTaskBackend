using AuthServices.Api.Utils;
using AuthServices.Application.DTOs;
using AuthServices.Application.DTOs.Erros;
using AuthServices.Application.Interface;
using AuthServices.Application.Model;
using AuthServices.Application.Model.Role;
using AuthServices.Application.Model.User;
using AuthServices.Application.Utils;
using AuthServices.Infraestructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthServices.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        public readonly IRoleService _roleService;
        public RoleController(IRoleService roleService) {
        
            _roleService = roleService;
        }

        [HttpPost("createdRol")]
        [ProducesResponseType(typeof(MsDtoResponse<RolResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult> CreatedAccount([FromBody] RoleRequest request)
        {
            var result = await _roleService.CreatedRole(request);

            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.RolCreated));
        }


        [HttpGet("listRoles")]
        [ProducesResponseType(typeof(ListDtos), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<IActionResult> GetRoles([FromQuery] ListDtos queryParams)
        {
            var result = await _roleService.GetRolesAsync(queryParams);
            return Ok(result);
        }




    }
}
