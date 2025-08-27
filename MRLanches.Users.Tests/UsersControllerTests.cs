using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MRLanches.Users.API.Controllers;
using MRLanches.Users.API.Interfaces;
using MRLanches.Users.API.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace MRLanches.Users.Tests
{
    public class UsersControllerTests
    {
        private UsersController GetControllerWithUser(IUserService service, string role, int userId)
        {
            var controller = new UsersController(service);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            return controller;
        }

        // --- CreateUser ---
        [Fact]
        public async Task CreateUser_Success()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserRequest>())).ReturnsAsync(new User { Id = 1, Username = "usuario1" });
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.CreateUser(new CreateUserRequest { Username = "usuario1", Password = "1234567" });
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task CreateUser_BusinessError()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserRequest>())).ThrowsAsync(new InvalidOperationException("Usuário já existe"));
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.CreateUser(new CreateUserRequest { Username = "usuario1", Password = "1234567" });
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("Usuário já existe", badRequest.Value?.ToString() ?? string.Empty);
        }

        [Fact]
        public async Task CreateUser_InternalError()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserRequest>())).ThrowsAsync(new System.Exception());
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.CreateUser(new CreateUserRequest { Username = "usuario1", Password = "1234567" });
            var error = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, error.StatusCode);
        }

        // --- UpdateUser ---
        [Fact]
        public async Task UpdateUser_Success()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.UpdateUserAsync(1, It.IsAny<UpdateUserRequest>())).ReturnsAsync(new User { Id = 1, Username = "usuario1" });
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.UpdateUser(1, new UpdateUserRequest { Username = "usuario1" });
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateUser_BusinessError()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.UpdateUserAsync(1, It.IsAny<UpdateUserRequest>())).ThrowsAsync(new InvalidOperationException("Não encontrado"));
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.UpdateUser(1, new UpdateUserRequest { Username = "usuario1" });
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("Não encontrado", badRequest.Value?.ToString() ?? string.Empty);
        }

        [Fact]
        public async Task UpdateUser_InternalError()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.UpdateUserAsync(1, It.IsAny<UpdateUserRequest>())).ThrowsAsync(new System.Exception());
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.UpdateUser(1, new UpdateUserRequest { Username = "usuario1" });
            var error = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, error.StatusCode);
        }

        // --- DeleteUser ---
        [Fact]
        public async Task DeleteUser_Success()
        {
            var mockService = new Moq.Mock<IUserService>();
            mockService.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(true);
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.DeleteUser(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteUser_NotFound()
        {
            var mockService = new Moq.Mock<IUserService>();
            mockService.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(false);
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.DeleteUser(1);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteUser_InternalError()
        {
            var mockService = new Moq.Mock<IUserService>();
            mockService.Setup(s => s.DeleteUserAsync(1)).ThrowsAsync(new System.Exception());
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.DeleteUser(1);
            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);
        }

        // --- GetUserById ---
        [Fact]
        public async Task GetUserById_ADM_Success()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(new User { Id = 1, Username = "usuario1", UserType = UserType.User });
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.GetUserById(1);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetUserById_ADM_NotFound()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync((User?)null);
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.GetUserById(1);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetUserById_MR_Success_UserCommon()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(new User { Id = 1, Username = "usuario1", UserType = UserType.User });
            var controller = GetControllerWithUser(mockService.Object, "MR", 99);
            var result = await controller.GetUserById(1);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetUserById_MR_NotFoundOrNotCommon()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(new User { Id = 1, Username = "usuario1", UserType = UserType.ADM });
            var controller = GetControllerWithUser(mockService.Object, "MR", 99);
            var result = await controller.GetUserById(1);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetUserById_User_Self_Success()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.GetUserByIdAsync(5)).ReturnsAsync(new User { Id = 5, Username = "eu", UserType = UserType.User });
            var controller = GetControllerWithUser(mockService.Object, "User", 5);
            var result = await controller.GetUserById(5);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetUserById_User_Self_NotFound()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.GetUserByIdAsync(5)).ReturnsAsync((User?)null);
            var controller = GetControllerWithUser(mockService.Object, "User", 5);
            var result = await controller.GetUserById(5);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetUserById_User_Other_Forbid()
        {
            var mockService = new Mock<IUserService>();
            var controller = GetControllerWithUser(mockService.Object, "User", 5);
            var result = await controller.GetUserById(10);
            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public async Task GetUserById_InternalError()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.GetUserByIdAsync(It.IsAny<int>())).ThrowsAsync(new System.Exception());
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.GetUserById(1);
            var error = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, error.StatusCode);
        }

        // --- GetAllUsers ---
        [Fact]
        public async Task GetAllUsers_Success()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(new List<User> { new User { Id = 1, Username = "usuario1" } });
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.GetAllUsers();
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetAllUsers_InternalError()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.GetAllUsersAsync()).ThrowsAsync(new System.Exception());
            var controller = GetControllerWithUser(mockService.Object, "ADM", 99);
            var result = await controller.GetAllUsers();
            var error = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, error.StatusCode);
        }
    }
}
