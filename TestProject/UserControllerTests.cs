using AuthServices.Api.Controllers;
using AuthServices.Application.DTOs;
using AuthServices.Application.DTOs.Erros;
using AuthServices.Application.DTOs.Users;
using AuthServices.Application.Interface;
using AuthServices.Application.Model.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class UserControllerTests
    {
        [Fact]
        public async Task UpdateUser_ReturnsOkResult_WithUpdatedUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateRequest = new UpdateUserRequest
            {
                UserID = userId,
                FirstName = "Updated",
                LastName = "User",
                Dni = "1234567890",
                Phone = "0987654321",
                AccountId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
                ModifiedBy = "admin@example.com"
            };

            var updatedUser = new UserResponse
            {
                UserID = userId
               
            };

            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.UpdateUser(updateRequest)).ReturnsAsync(updatedUser);

            var controller = new UserController(mockService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            // Act
            var result = await controller.UpdateUser(updateRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<MsDtoResponse<UserResponse>>(okResult.Value);
            Assert.Equal(updateRequest.UserID.ToString(), response.data.UserID.ToString());
        }

        [Fact]
        public async Task DeleteUser_ReturnsOkResult_WithConfirmationMessage()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var successMessage = "User successfully deleted.";

            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.DeleteUserAsync(userId));

            var controller = new UserController(mockService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            // Act
            var result = await controller.DeleteUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<MsDtoResponse<bool>>(okResult.Value);
            Assert.Equal(successMessage, response.message);
        }
    }
}
