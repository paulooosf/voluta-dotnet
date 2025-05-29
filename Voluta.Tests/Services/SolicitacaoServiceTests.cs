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
        private readonly ISolicitacaoService _solicitacaoService;

        public SolicitacaoServiceTests()
        {
            _solicitacaoRepositoryMock = new Mock<ISolicitacaoVoluntariadoRepository>();
            _ongRepositoryMock = new Mock<IOngRepository>();

            _solicitacaoService = new SolicitacaoService(
                _solicitacaoRepositoryMock.Object,
                _ongRepositoryMock.Object);
        }

        [Fact]
        public async Task GetSolicitacoesOngAsync_QuandoOngNaoExiste_DeveLancarExcecao()
        {
            // Arrange
            _ongRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Ong)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ErroNaoEncontrado>(
                () => _solicitacaoService.GetSolicitacoesOngAsync(1, null, 1, 10));
            Assert.Equal("ONG com ID 1 não foi encontrada", exception.Message);
        }

        [Fact]
        public async Task GetSolicitacoesOngAsync_QuandoOngExiste_DeveRetornarSolicitacoesPaginadas()
        {
            // Arrange
            var ong = new Ong
            {
                Id = 1,
                Nome = "Teste ONG"
            };

            var usuario = new Usuario
            {
                Id = 1,
                Nome = "Teste Usuario"
            };

            var solicitacoes = new List<SolicitacaoVoluntariado>
            {
                new SolicitacaoVoluntariado
                {
                    Id = 1,
                    UsuarioId = 1,
                    OngId = 1,
                    Status = StatusSolicitacao.Pendente,
                    DataSolicitacao = DateTime.Now,
                    Mensagem = "Teste",
                    Usuario = usuario,
                    Ong = ong
                }
            };

            _ongRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(ong);
            _solicitacaoRepositoryMock.Setup(r => r.GetByOngAsync(1, null, 0, 10))
                .ReturnsAsync(solicitacoes);
            _solicitacaoRepositoryMock.Setup(r => r.GetTotalByOngAsync(1, null))
                .ReturnsAsync(1);

            // Act
            var resultado = await _solicitacaoService.GetSolicitacoesOngAsync(1, null, 1, 10);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado.Items);
            Assert.Equal(1, resultado.TotalItems);
            Assert.Equal(1, resultado.TotalPages);
            Assert.Equal(1, resultado.PageNumber);
            Assert.Equal(10, resultado.PageSize);

            var solicitacao = resultado.Items.First();
            Assert.Equal(1, solicitacao.Id);
            Assert.Equal(1, solicitacao.UsuarioId);
            Assert.Equal("Teste Usuario", solicitacao.NomeUsuario);
            Assert.Equal(1, solicitacao.OngId);
            Assert.Equal("Teste ONG", solicitacao.NomeOng);
            Assert.Equal(StatusSolicitacao.Pendente, solicitacao.Status);
        }

        [Fact]
        public async Task GetSolicitacoesOngAsync_ComFiltroStatus_DeveRetornarSolicitacoesFiltradas()
        {
            // Arrange
            var ong = new Ong
            {
                Id = 1,
                Nome = "Teste ONG"
            };

            var usuario = new Usuario
            {
                Id = 1,
                Nome = "Teste Usuario"
            };

            var solicitacoes = new List<SolicitacaoVoluntariado>
            {
                new SolicitacaoVoluntariado
                {
                    Id = 1,
                    UsuarioId = 1,
                    OngId = 1,
                    Status = StatusSolicitacao.Aprovada,
                    DataSolicitacao = DateTime.Now,
                    DataAprovacao = DateTime.Now,
                    Mensagem = "Teste",
                    Usuario = usuario,
                    Ong = ong
                }
            };

            var statusFiltro = StatusSolicitacao.Aprovada;

            _ongRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(ong);
            _solicitacaoRepositoryMock.Setup(r => r.GetByOngAsync(1, statusFiltro, 0, 10))
                .ReturnsAsync(solicitacoes);
            _solicitacaoRepositoryMock.Setup(r => r.GetTotalByOngAsync(1, statusFiltro))
                .ReturnsAsync(1);

            // Act
            var resultado = await _solicitacaoService.GetSolicitacoesOngAsync(1, statusFiltro, 1, 10);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado.Items);
            Assert.Equal(1, resultado.TotalItems);

            var solicitacao = resultado.Items.First();
            Assert.Equal(StatusSolicitacao.Aprovada, solicitacao.Status);
            Assert.NotNull(solicitacao.DataAprovacao);
        }

        [Fact]
        public async Task GetSolicitacoesOngAsync_QuandoNaoHaSolicitacoes_DeveRetornarListaVazia()
        {
            // Arrange
            var ong = new Ong
            {
                Id = 1,
                Nome = "Teste ONG"
            };

            _ongRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(ong);
            _solicitacaoRepositoryMock.Setup(r => r.GetByOngAsync(1, null, 0, 10))
                .ReturnsAsync(new List<SolicitacaoVoluntariado>());
            _solicitacaoRepositoryMock.Setup(r => r.GetTotalByOngAsync(1, null))
                .ReturnsAsync(0);

            // Act
            var resultado = await _solicitacaoService.GetSolicitacoesOngAsync(1, null, 1, 10);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado.Items);
            Assert.Equal(0, resultado.TotalItems);
            Assert.Equal(0, resultado.TotalPages);
        }

        [Fact]
        public async Task AprovarSolicitacao_QuandoSolicitacaoNaoExiste_DeveLancarExcecao()
        {
            // Arrange
            _solicitacaoRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((SolicitacaoVoluntariado)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ErroNaoEncontrado>(
                () => _solicitacaoService.AprovarSolicitacaoAsync(1));
            Assert.Equal("Solicitação com ID 1 não foi encontrada", exception.Message);
        }

        [Fact]
        public async Task AprovarSolicitacao_QuandoSolicitacaoNaoEstaPendente_DeveLancarExcecao()
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
            var exception = await Assert.ThrowsAsync<ErroNegocio>(
                () => _solicitacaoService.AprovarSolicitacaoAsync(1));
            Assert.Equal("Apenas solicitações pendentes podem ser aprovadas", exception.Message);
        }

        [Fact]
        public async Task RejeitarSolicitacao_QuandoSolicitacaoNaoExiste_DeveLancarExcecao()
        {
            // Arrange
            _solicitacaoRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((SolicitacaoVoluntariado)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ErroNaoEncontrado>(
                () => _solicitacaoService.RejeitarSolicitacaoAsync(1));
            Assert.Equal("Solicitação com ID 1 não foi encontrada", exception.Message);
        }

        [Fact]
        public async Task RejeitarSolicitacao_QuandoSolicitacaoNaoEstaPendente_DeveLancarExcecao()
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
            var exception = await Assert.ThrowsAsync<ErroNegocio>(
                () => _solicitacaoService.RejeitarSolicitacaoAsync(1));
            Assert.Equal("Apenas solicitações pendentes podem ser rejeitadas", exception.Message);
        }
    }
} 