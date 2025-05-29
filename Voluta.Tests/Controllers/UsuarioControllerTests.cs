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
            Assert.Equal(paginatedResult, okResult.Value);
        }

        [Fact]
        public async Task GetUsuario_ReturnsOkResult()
        {
            // Arrange
            var usuario = new UsuarioViewModel
            {
                Id = 1,
                Nome = "Teste Usuario"
            };

            _usuarioServiceMock.Setup(x => x.GetUsuarioAsync(1))
                .ReturnsAsync(usuario);

            // Act
            var result = await _controller.GetUsuario(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(usuario, okResult.Value);
        }

        [Fact]
        public async Task SolicitarVoluntariado_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var model = new NovaSolicitacaoVoluntariadoViewModel
            {
                OngId = 1,
                Mensagem = "Teste"
            };

            var solicitacao = new SolicitacaoVoluntariadoViewModel
            {
                Id = 1,
                UsuarioId = 1,
                OngId = 1,
                Mensagem = "Teste"
            };

            _usuarioServiceMock.Setup(x => x.SolicitarVoluntariadoAsync(1, model))
                .ReturnsAsync(solicitacao);

            // Act
            var result = await _controller.SolicitarVoluntariado(model, 1);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(nameof(UsuarioController.GetSolicitacao), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(solicitacao, createdAtActionResult.Value);
        }

        [Fact]
        public async Task GetSolicitacao_ReturnsOkResult()
        {
            // Arrange
            var solicitacao = new SolicitacaoVoluntariadoViewModel
            {
                Id = 1,
                UsuarioId = 1,
                OngId = 1,
                Mensagem = "Teste"
            };

            _usuarioServiceMock.Setup(x => x.GetSolicitacaoAsync(1))
                .ReturnsAsync(solicitacao);

            // Act
            var result = await _controller.GetSolicitacao(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(solicitacao, okResult.Value);
        }
    }
} 