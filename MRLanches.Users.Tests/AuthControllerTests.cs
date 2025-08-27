using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MRLanches.Users.API.Controllers;
using MRLanches.Users.API.Interfaces;
using MRLanches.Users.API.Models;
using System.Threading.Tasks;

namespace MRLanches.Users.Tests
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_Success()
        {
            var mockService = new Mock<IAuthService>();
            mockService.Setup(s => s.AuthenticateAsync(It.IsAny<AuthRequest>())).ReturnsAsync(new AuthResponse { Success = true });
            var controller = new AuthController(mockService.Object);
            var result = await controller.Login(new AuthRequest { Username = "ADM", Password = "1234567" });
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Login_Fail()
        {
            var mockService = new Mock<IAuthService>();
            mockService.Setup(s => s.AuthenticateAsync(It.IsAny<AuthRequest>())).ReturnsAsync(new AuthResponse { Success = false });
            var controller = new AuthController(mockService.Object);
            var result = await controller.Login(new AuthRequest { Username = "ADM", Password = "errado" });
            Assert.IsType<UnauthorizedObjectResult>(result.Result);
        }
    }
}
