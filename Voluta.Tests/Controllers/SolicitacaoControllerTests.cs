using Microsoft.AspNetCore.Mvc;
using Moq;
using Voluta.Controllers;
using Voluta.Models;
using Voluta.Services;
using Voluta.ViewModels;
using Xunit;

namespace Voluta.Tests.Controllers
{
    public class SolicitacaoControllerTests
    {
        private readonly Mock<ISolicitacaoService> _solicitacaoServiceMock;
        private readonly SolicitacaoController _controller;

        public SolicitacaoControllerTests()
        {
            _solicitacaoServiceMock = new Mock<ISolicitacaoService>();
            _controller = new SolicitacaoController(_solicitacaoServiceMock.Object);
        }

        [Fact]
        public async Task GetSolicitacoesOng_ReturnsOkResult()
        {
            // Arrange
            var paginatedResult = new PaginatedViewModel<SolicitacaoVoluntariadoViewModel>
            {
                Items = new List<SolicitacaoVoluntariadoViewModel>(),
                PageNumber = 1,
                PageSize = 10,
                TotalPages = 1,
                TotalItems = 0
            };

            _solicitacaoServiceMock.Setup(x => x.GetSolicitacoesOngAsync(1, null, 1, 10))
                .ReturnsAsync(paginatedResult);

            // Act
            var result = await _controller.GetSolicitacoesOng(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(paginatedResult, okResult.Value);
        }

        [Fact]
        public async Task AprovarSolicitacao_ReturnsNoContentResult()
        {
            // Arrange
            _solicitacaoServiceMock.Setup(x => x.AprovarSolicitacaoAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AprovarSolicitacao(1);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task RejeitarSolicitacao_ReturnsNoContentResult()
        {
            // Arrange
            _solicitacaoServiceMock.Setup(x => x.RejeitarSolicitacaoAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RejeitarSolicitacao(1);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }
    }
} 