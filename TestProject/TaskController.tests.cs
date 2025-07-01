using AuthServices.Api.Controllers;
using AuthServices.Application.DTOs;
using AuthServices.Application.DTOs.Erros;
using AuthServices.Application.Interface;
using AuthServices.Application.Model.Task;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class TaskControllerTest
    {
        [Fact]
        public async Task CreatedTask_ReturnsOkResult_WithCreatedTask()
        {
            // Arrange
            var taskRequest = new TaskRequest
            {
                Title = "Nueva tarea",
                Description = "Descripción de prueba",
                CreatedBy = "admin"
            };

            var taskId = Guid.NewGuid();

            var taskResponse = new TaskResponse
            {
                TaskID = taskId,
               
            };

            var mockService = new Mock<ITaskService>();
            mockService.Setup(s => s.CreatedTask(taskRequest)).ReturnsAsync(taskResponse);

            var controller = new TaskController(mockService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            // Act
            var result = await controller.CreatedAccount(taskRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<MsDtoResponse<TaskResponse>>(okResult.Value);
            Assert.Equal(taskId, response.data.TaskID);
        }

        [Fact]
        public async Task UpdateTask_ReturnsOkResult_WithUpdatedTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var successMessage = "Task successfully updated.";

            var updateRequest = new UpdateTaskRequest
            {
                TaskId = taskId,
                Title = "Tarea actualizada",
                Description = "Descripción actualizada",
                Status = "En Proceso",
                UpdatedBy = "admin"
            };

            var updatedTask = new TaskResponse
            {
                TaskID = taskId,
               
            };

            var mockService = new Mock<ITaskService>();
            mockService.Setup(s => s.UpdateTask(updateRequest)).ReturnsAsync(updatedTask);

            var controller = new TaskController(mockService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            // Act
            var result = await controller.UpdateTask(updateRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<MsDtoResponse<TaskResponse>>(okResult.Value);
            Assert.Equal(successMessage, response.message);
        }


        [Fact]
        public async Task DeleteTask_ReturnsOkResult_WithSuccessMessage()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var expectedMessage = "Task successfully deleted.";

            var mockService = new Mock<ITaskService>();
            mockService
                .Setup(s => s.DeleteTaskAsync(taskId));

            var controller = new TaskController(mockService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            // Act
            var result = await controller.DeleteUser(taskId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<MsDtoResponse<bool>>(okResult.Value);
            Assert.Equal(expectedMessage, response.message);
        }


    }
}
