using Microsoft.AspNetCore.Mvc;
using Moq;
using Voluta.Controllers;
using Voluta.Services;
using Voluta.ViewModels;
using Xunit;

namespace Voluta.Tests.Controllers
{
    public class UsuarioControllerTests
    {
        private readonly Mock<IUsuarioService> _usuarioServiceMock;
        private readonly UsuarioController _controller;

        public UsuarioControllerTests()
        {
            _usuarioServiceMock = new Mock<IUsuarioService>();
            _controller = new UsuarioController(_usuarioServiceMock.Object);
        }

        [Fact]
        public async Task GetUsuarios_ReturnsOkResult()
        {
            // Arrange
            var paginatedResult = new PaginatedViewModel<UsuarioViewModel>
            {
                Items = new List<UsuarioViewModel>(),
                PageNumber = 1,
                PageSize = 10,
                TotalPages = 1,
                TotalItems = 0
            };

            _usuarioServiceMock.Setup(x => x.GetUsuariosAsync(1, 10))
                .ReturnsAsync(paginatedResult);

            // Act
            var result = await _controller.GetUsuarios();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }
    }
} 