using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Voluta.Models;
using Voluta.Repositories;
using Voluta.Services;
using Voluta.ViewModels;
using Voluta.Exceptions;
using Xunit;

namespace Voluta.Tests.Services
{
    public class SolicitacaoServiceTests
    {
        private readonly Mock<ISolicitacaoVoluntariadoRepository> _solicitacaoRepositoryMock;
        private readonly Mock<IOngRepository> _ongRepositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly SolicitacaoService _service;

        public SolicitacaoServiceTests()
        {
            _solicitacaoRepositoryMock = new Mock<ISolicitacaoVoluntariadoRepository>();
            _ongRepositoryMock = new Mock<IOngRepository>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _service = new SolicitacaoService(
                _solicitacaoRepositoryMock.Object,
                _ongRepositoryMock.Object,
                _usuarioRepositoryMock.Object);
        }

        [Fact]
        public async Task GetSolicitacoesAsync_DeveRetornarPaginado()
        {
            // Arrange
            var solicitacoes = new List<SolicitacaoVoluntariado>
            {
                new SolicitacaoVoluntariado { Id = 1, Status = StatusSolicitacao.Pendente },
                new SolicitacaoVoluntariado { Id = 2, Status = StatusSolicitacao.Aprovada }
            };

            _solicitacaoRepositoryMock.Setup(r => r.GetAllAsync(
                    It.IsAny<StatusSolicitacao?>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .ReturnsAsync(solicitacoes);

            _solicitacaoRepositoryMock.Setup(r => r.GetTotalCountAsync(
                    It.IsAny<StatusSolicitacao?>()))
                .ReturnsAsync(solicitacoes.Count);

            // Act
            var resultado = await _service.GetSolicitacoesAsync(null, 1, 10);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.PageNumber);
            Assert.Equal(10, resultado.PageSize);
            Assert.Equal(2, resultado.TotalItems);
            Assert.Equal(1, resultado.TotalPages);
            Assert.Equal(2, resultado.Items.Count());
        }

        [Fact]
        public async Task GetSolicitacaoAsync_QuandoExiste_DeveRetornar()
        {
            // Arrange
            var solicitacao = new SolicitacaoVoluntariado
            {
                Id = 1,
                Status = StatusSolicitacao.Pendente,
                Usuario = new Usuario { Id = 1, Nome = "Teste" },
                Ong = new Ong { Id = 1, Nome = "ONG Teste" }
            };

            _solicitacaoRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(solicitacao);

            // Act
            var resultado = await _service.GetSolicitacaoAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal(StatusSolicitacao.Pendente, resultado.Status);
        }

        [Fact]
        public async Task GetSolicitacaoAsync_QuandoNaoExiste_DeveLancarErro()
        {
            // Arrange
            _solicitacaoRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((SolicitacaoVoluntariado)null);

            // Act & Assert
            await Assert.ThrowsAsync<ErroNaoEncontrado>(
                () => _service.GetSolicitacaoAsync(1));
        }

        [Fact]
        public async Task GetSolicitacoesOngAsync_DeveRetornarPaginado()
        {
            // Arrange
            var ong = new Ong { Id = 1, Nome = "ONG Teste" };
            var solicitacoes = new List<SolicitacaoVoluntariado>
            {
                new SolicitacaoVoluntariado { Id = 1, OngId = 1, Status = StatusSolicitacao.Pendente },
                new SolicitacaoVoluntariado { Id = 2, OngId = 1, Status = StatusSolicitacao.Aprovada }
            };

            _ongRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(ong);

            _solicitacaoRepositoryMock.Setup(r => r.GetByOngAsync(
                    It.IsAny<int>(),
                    It.IsAny<StatusSolicitacao?>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .ReturnsAsync(solicitacoes);

            _solicitacaoRepositoryMock.Setup(r => r.GetTotalByOngAsync(
                    It.IsAny<int>(),
                    It.IsAny<StatusSolicitacao?>()))
                .ReturnsAsync(solicitacoes.Count);

            // Act
            var resultado = await _service.GetSolicitacoesOngAsync(1, null, 1, 10);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.PageNumber);
            Assert.Equal(10, resultado.PageSize);
            Assert.Equal(2, resultado.TotalItems);
            Assert.Equal(1, resultado.TotalPages);
            Assert.Equal(2, resultado.Items.Count());
        }

        [Fact]
        public async Task GetSolicitacoesUsuarioAsync_DeveRetornarPaginado()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Nome = "Usuário Teste" };
            var solicitacoes = new List<SolicitacaoVoluntariado>
            {
                new SolicitacaoVoluntariado { Id = 1, UsuarioId = 1, Status = StatusSolicitacao.Pendente },
                new SolicitacaoVoluntariado { Id = 2, UsuarioId = 1, Status = StatusSolicitacao.Aprovada }
            };

            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(usuario);

            _solicitacaoRepositoryMock.Setup(r => r.GetByUsuarioAsync(
                    It.IsAny<int>(),
                    It.IsAny<StatusSolicitacao?>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .ReturnsAsync(solicitacoes);

            _solicitacaoRepositoryMock.Setup(r => r.GetTotalByUsuarioAsync(
                    It.IsAny<int>(),
                    It.IsAny<StatusSolicitacao?>()))
                .ReturnsAsync(solicitacoes.Count);

            // Act
            var resultado = await _service.GetSolicitacoesUsuarioAsync(1, null, 1, 10);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.PageNumber);
            Assert.Equal(10, resultado.PageSize);
            Assert.Equal(2, resultado.TotalItems);
            Assert.Equal(1, resultado.TotalPages);
            Assert.Equal(2, resultado.Items.Count());
        }

        [Fact]
        public async Task AprovarSolicitacaoAsync_QuandoPendente_DeveAprovar()
        {
            // Arrange
            var solicitacao = new SolicitacaoVoluntariado
            {
                Id = 1,
                Status = StatusSolicitacao.Pendente
            };

            _solicitacaoRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(solicitacao);

            // Act
            await _service.AprovarSolicitacaoAsync(1);

            // Assert
            Assert.Equal(StatusSolicitacao.Aprovada, solicitacao.Status);
            Assert.NotNull(solicitacao.DataAprovacao);
            _solicitacaoRepositoryMock.Verify(r => r.UpdateAsync(solicitacao), Times.Once);
        }

        [Fact]
        public async Task AprovarSolicitacaoAsync_QuandoNaoPendente_DeveLancarErro()
        {
            // Arrange
            var solicitacao = new SolicitacaoVoluntariado
            {
                Id = 1,
                Status = StatusSolicitacao.Aprovada
            };

            _solicitacaoRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(solicitacao);

            // Act & Assert
            await Assert.ThrowsAsync<ErroNegocio>(
                () => _service.AprovarSolicitacaoAsync(1));
        }

        [Fact]
        public async Task RejeitarSolicitacaoAsync_QuandoPendente_DeveRejeitar()
        {
            // Arrange
            var solicitacao = new SolicitacaoVoluntariado
            {
                Id = 1,
                Status = StatusSolicitacao.Pendente
            };

            _solicitacaoRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(solicitacao);

            // Act
            await _service.RejeitarSolicitacaoAsync(1);

            // Assert
            Assert.Equal(StatusSolicitacao.Rejeitada, solicitacao.Status);
            _solicitacaoRepositoryMock.Verify(r => r.UpdateAsync(solicitacao), Times.Once);
        }

        [Fact]
        public async Task RejeitarSolicitacaoAsync_QuandoNaoPendente_DeveLancarErro()
        {
            // Arrange
            var solicitacao = new SolicitacaoVoluntariado
            {
                Id = 1,
                Status = StatusSolicitacao.Aprovada
            };

            _solicitacaoRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(solicitacao);

            // Act & Assert
            await Assert.ThrowsAsync<ErroNegocio>(
                () => _service.RejeitarSolicitacaoAsync(1));
        }

        [Fact]
        public async Task DeleteSolicitacaoAsync_QuandoAprovada_DeveLancarErro()
        {
            // Arrange
            var solicitacao = new SolicitacaoVoluntariado
            {
                Id = 1,
                Status = StatusSolicitacao.Aprovada
            };

            _solicitacaoRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(solicitacao);

            // Act & Assert
            await Assert.ThrowsAsync<ErroNegocio>(
                () => _service.DeleteSolicitacaoAsync(1));
        }

        [Fact]
        public async Task DeleteSolicitacaoAsync_QuandoNaoAprovada_DeveDeletar()
        {
            // Arrange
            var solicitacao = new SolicitacaoVoluntariado
            {
                Id = 1,
                Status = StatusSolicitacao.Rejeitada
            };

            _solicitacaoRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(solicitacao);

            // Act
            await _service.DeleteSolicitacaoAsync(1);

            // Assert
            _solicitacaoRepositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
} 