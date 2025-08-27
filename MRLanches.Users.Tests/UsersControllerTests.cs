using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MRLanches.Users.API.Controllers;
using MRLanches.Users.API.Interfaces;
using MRLanches.Users.API.Models;
using System.Threading.Tasks;

namespace MRLanches.Users.Tests
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task CreateUser_Success()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserRequest>())).ReturnsAsync(new User { Id = 1, Username = "usuario1" });
            var controller = new UsersController(mockService.Object);
            var result = await controller.CreateUser(new CreateUserRequest { Username = "usuario1", Password = "1234567" });
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }
        // Adicione outros testes para UpdateUser, DeleteUser, GetUserById, GetAllUsers, cobrindo casos de sucesso e falha
    }
}
