using Microsoft.AspNetCore.Mvc;
using Moq;
using Voluta.Controllers;
using Voluta.Services;
using Voluta.ViewModels;
using Xunit;

namespace Voluta.Tests.Controllers
{
    public class OngControllerTests
    {
        private readonly Mock<IOngService> _ongServiceMock;
        private readonly OngController _controller;

        public OngControllerTests()
        {
            _ongServiceMock = new Mock<IOngService>();
            _controller = new OngController(_ongServiceMock.Object);
        }

        [Fact]
        public async Task GetOngs_ReturnsOkResult()
        {
            // Arrange
            var paginatedResult = new PaginatedViewModel<OngViewModel>
            {
                Items = new List<OngViewModel>(),
                PageNumber = 1,
                PageSize = 10,
                TotalPages = 1,
                TotalItems = 0
            };

            _ongServiceMock.Setup(x => x.GetOngsAsync(1, 10))
                .ReturnsAsync(paginatedResult);

            // Act
            var result = await _controller.GetOngs();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(paginatedResult, okResult.Value);
        }

        [Fact]
        public async Task GetOng_ReturnsOkResult()
        {
            // Arrange
            var ong = new OngViewModel
            {
                Id = 1,
                Nome = "Teste ONG"
            };

            _ongServiceMock.Setup(x => x.GetOngAsync(1))
                .ReturnsAsync(ong);

            // Act
            var result = await _controller.GetOng(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(ong, okResult.Value);
        }

        [Fact]
        public async Task GetVoluntariosDisponiveis_ReturnsOkResult()
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

            _ongServiceMock.Setup(x => x.GetVoluntariosDisponiveisAsync(1, 1, 10))
                .ReturnsAsync(paginatedResult);

            // Act
            var result = await _controller.GetVoluntariosDisponiveis(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(paginatedResult, okResult.Value);
        }
    }
} 