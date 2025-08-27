using Xunit;
using Microsoft.AspNetCore.Mvc;
using MRLanches.Users.API.Controllers;

namespace MRLanches.Users.Tests
{
    public class TempAuthControllerTests
    {
        [Fact]
        public void Test_ReturnsOk()
        {
            var controller = new TempAuthController();
            var result = controller.Test();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("API funcionando! Swagger está ativo.", okResult.Value);
        }

        [Fact]
        public void Hello_ReturnsOk()
        {
            var controller = new TempAuthController();
            var result = controller.Hello("Teste");
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Contains("Olá Teste!", okResult.Value?.ToString() ?? string.Empty);
        }

        [Fact]
        public void Echo_ReturnsOk()
        {
            var controller = new TempAuthController();
            var data = new { nome = "teste" };
            var result = controller.Echo(data);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
        }
    }
}
