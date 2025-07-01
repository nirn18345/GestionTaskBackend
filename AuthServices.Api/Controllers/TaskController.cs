using AuthServices.Api.Utils;
using AuthServices.Application.DTOs;
using AuthServices.Application.DTOs.Erros;
using AuthServices.Application.DTOs.Users;
using AuthServices.Application.Interface;
using AuthServices.Application.Model.Task;
using AuthServices.Application.Model.User;
using AuthServices.Application.Utils;
using AuthServices.Infraestructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace AuthServices.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        public readonly ITaskService _TaskService;


        public TaskController(ITaskService TaskService)
        {
            _TaskService = TaskService;
        }


        [HttpPost("createdTask")]
        [ProducesResponseType(typeof(MsDtoResponse<TaskResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult> CreatedAccount([FromBody] TaskRequest request)
        {
            var result = await _TaskService.CreatedTask(request);

            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.TaskCreated));
        }


        [HttpPut("updatedTask")]
        [ProducesResponseType(typeof(MsDtoResponse<UserResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult> UpdateTask([FromBody] UpdateTaskRequest request)
        {
            var result = await _TaskService.UpdateTask(request);

            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.TaskUpdate));
        }


        [HttpGet("listTask")]
        [ProducesResponseType(typeof(ListDtos), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<IActionResult> GetUsers([FromQuery] ListDtos queryParams)
        {
            var result = await _TaskService.GetTasksAsync(queryParams);
            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.TaskList));
        }



        [HttpGet("getTaskById")]
        [ProducesResponseType(typeof(UserListDto), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<IActionResult> GetUsersbyID(Guid TaskId)
        {
            var result = await _TaskService.GetTaskByIdAsync(TaskId);
            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.TaskFound));
        }


        [HttpGet("getTasksByUserId")]
        [ProducesResponseType(typeof(ListDtos), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<IActionResult> getTaskByUserId([FromQuery]ListUsetDtos queryParams)
        {
            var result = await _TaskService.GetTaskByUserIdAsync(queryParams);
            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.TaskFound));
        }


        [HttpDelete("deleteTask/{id}")]
        [ProducesResponseType(typeof(MsDtoResponse<string>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 404)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var result = await _TaskService.DeleteTaskAsync(id);

            return Ok(ApiResponseBuilder.Success(result, HttpContext, ResponseMessage.TaskDelete));
        }

    }
}
