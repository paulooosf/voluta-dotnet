using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Voluta.Models;
using Voluta.Repositories;
using Voluta.Services;
using Voluta.ViewModels;
using Xunit;

namespace Voluta.Tests.Services
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IOngRepository> _ongRepositoryMock;
        private readonly Mock<ISolicitacaoVoluntariadoRepository> _solicitacaoRepositoryMock;
        private readonly IUsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _ongRepositoryMock = new Mock<IOngRepository>();
            _solicitacaoRepositoryMock = new Mock<ISolicitacaoVoluntariadoRepository>();

            _usuarioService = new UsuarioService(
                _usuarioRepositoryMock.Object,
                _ongRepositoryMock.Object,
                _solicitacaoRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUsuariosAsync_DeveRetornarUsuariosPaginados()
        {
            // Arrange
            var usuarios = new List<Usuario>
            {
                new Usuario
                {
                    Id = 1,
                    Nome = "Teste Usuario",
                    Email = "teste@email.com",
                    Telefone = "123456789",
                    Disponivel = true,
                    AreasInteresse = new[] { AreaAtuacao.EducacaoEnsino },
                    DataCadastro = DateTime.Now
                }
            };

            _usuarioRepositoryMock.Setup(r => r.GetAllAsync(0, 10))
                .ReturnsAsync(usuarios);
            _usuarioRepositoryMock.Setup(r => r.GetTotalCountAsync())
                .ReturnsAsync(1);

            // Act
            var resultado = await _usuarioService.GetUsuariosAsync(1, 10);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado.Items);
            Assert.Equal(1, resultado.TotalItems);
            Assert.Equal(1, resultado.TotalPages);
            Assert.Equal(1, resultado.PageNumber);
            Assert.Equal(10, resultado.PageSize);

            var usuario = resultado.Items.First();
            Assert.Equal(1, usuario.Id);
            Assert.Equal("Teste Usuario", usuario.Nome);
            Assert.Equal("teste@email.com", usuario.Email);
        }

        [Fact]
        public async Task SolicitarVoluntariadoAsync_QuandoUsuarioNaoExiste_DeveLancarExcecao()
        {
            // Arrange
            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Usuario)null);

            var model = new NovaSolicitacaoVoluntariadoViewModel
            {
                OngId = 1,
                Mensagem = "Teste"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _usuarioService.SolicitarVoluntariadoAsync(1, model));
            Assert.Equal("Usuário não encontrado", exception.Message);
        }

        [Fact]
        public async Task SolicitarVoluntariadoAsync_QuandoUsuarioNaoDisponivel_DeveLancarExcecao()
        {
            // Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Nome = "Teste Usuario",
                Disponivel = false
            };

            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(usuario);

            var model = new NovaSolicitacaoVoluntariadoViewModel
            {
                OngId = 1,
                Mensagem = "Teste"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _usuarioService.SolicitarVoluntariadoAsync(1, model));
            Assert.Equal("Usuário não está disponível para voluntariado", exception.Message);
        }

        [Fact]
        public async Task SolicitarVoluntariadoAsync_QuandoOngNaoExiste_DeveLancarExcecao()
        {
            // Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Nome = "Teste Usuario",
                Disponivel = true
            };

            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(usuario);
            _ongRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Ong)null);

            var model = new NovaSolicitacaoVoluntariadoViewModel
            {
                OngId = 1,
                Mensagem = "Teste"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _usuarioService.SolicitarVoluntariadoAsync(1, model));
            Assert.Equal("ONG não encontrada", exception.Message);
        }

        [Fact]
        public async Task SolicitarVoluntariadoAsync_QuandoJaExisteSolicitacaoPendente_DeveLancarExcecao()
        {
            // Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Nome = "Teste Usuario",
                Disponivel = true
            };

            var ong = new Ong
            {
                Id = 1,
                Nome = "Teste ONG"
            };

            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(usuario);
            _ongRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(ong);
            _solicitacaoRepositoryMock.Setup(r => r.ExistePendente(1, 1))
                .ReturnsAsync(true);

            var model = new NovaSolicitacaoVoluntariadoViewModel
            {
                OngId = 1,
                Mensagem = "Teste"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _usuarioService.SolicitarVoluntariadoAsync(1, model));
            Assert.Equal("Já existe uma solicitação pendente para esta ONG", exception.Message);
        }

        [Fact]
        public async Task SolicitarVoluntariadoAsync_QuandoTudoValido_DeveCriarSolicitacao()
        {
            // Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Nome = "Teste Usuario",
                Disponivel = true
            };

            var ong = new Ong
            {
                Id = 1,
                Nome = "Teste ONG"
            };

            var solicitacao = new SolicitacaoVoluntariado
            {
                Id = 1,
                UsuarioId = 1,
                OngId = 1,
                Mensagem = "Teste",
                Status = StatusSolicitacao.Pendente,
                DataSolicitacao = DateTime.Now,
                Usuario = usuario,
                Ong = ong
            };

            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(usuario);
            _ongRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(ong);
            _solicitacaoRepositoryMock.Setup(r => r.ExistePendente(1, 1))
                .ReturnsAsync(false);
            _solicitacaoRepositoryMock.Setup(r => r.AddAsync(It.IsAny<SolicitacaoVoluntariado>()))
                .ReturnsAsync(solicitacao);

            var model = new NovaSolicitacaoVoluntariadoViewModel
            {
                OngId = 1,
                Mensagem = "Teste"
            };

            // Act
            var resultado = await _usuarioService.SolicitarVoluntariadoAsync(1, model);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal(1, resultado.UsuarioId);
            Assert.Equal(1, resultado.OngId);
            Assert.Equal("Teste", resultado.Mensagem);
            Assert.Equal(StatusSolicitacao.Pendente, resultado.Status);
            Assert.Equal("Teste Usuario", resultado.NomeUsuario);
            Assert.Equal("Teste ONG", resultado.NomeOng);
        }
    }
} 