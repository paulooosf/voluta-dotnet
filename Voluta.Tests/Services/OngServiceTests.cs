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
    public class OngServiceTests
    {
        private readonly Mock<IOngRepository> _ongRepositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly IOngService _ongService;

        public OngServiceTests()
        {
            _ongRepositoryMock = new Mock<IOngRepository>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();

            _ongService = new OngService(
                _ongRepositoryMock.Object,
                _usuarioRepositoryMock.Object);
        }

        [Fact]
        public async Task GetOngsAsync_DeveRetornarOngsPaginadas()
        {
            // Arrange
            var ongs = new List<Ong>
            {
                new Ong
                {
                    Id = 1,
                    Nome = "Teste ONG",
                    Cnpj = "12345678901234",
                    Email = "ong@teste.com",
                    Telefone = "123456789",
                    Descricao = "ONG de teste",
                    AreasAtuacao = new[] { AreaAtuacao.EducacaoEnsino },
                    Endereco = "Rua Teste, 123",
                    DataCadastro = DateTime.Now
                }
            };

            _ongRepositoryMock.Setup(r => r.GetAllAsync(0, 10))
                .ReturnsAsync(ongs);
            _ongRepositoryMock.Setup(r => r.GetTotalCountAsync())
                .ReturnsAsync(1);

            // Act
            var resultado = await _ongService.GetOngsAsync(1, 10);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado.Items);
            Assert.Equal(1, resultado.TotalItems);
            Assert.Equal(1, resultado.TotalPages);
            Assert.Equal(1, resultado.PageNumber);
            Assert.Equal(10, resultado.PageSize);

            var ong = resultado.Items.First();
            Assert.Equal(1, ong.Id);
            Assert.Equal("Teste ONG", ong.Nome);
            Assert.Equal("ong@teste.com", ong.Email);
        }

        [Fact]
        public async Task GetOngAsync_QuandoOngExiste_DeveRetornarOng()
        {
            // Arrange
            var ong = new Ong
            {
                Id = 1,
                Nome = "Teste ONG",
                Cnpj = "12345678901234",
                Email = "ong@teste.com",
                Telefone = "123456789",
                Descricao = "ONG de teste",
                AreasAtuacao = new[] { AreaAtuacao.EducacaoEnsino },
                Endereco = "Rua Teste, 123",
                DataCadastro = DateTime.Now
            };

            _ongRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(ong);

            // Act
            var resultado = await _ongService.GetOngAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("Teste ONG", resultado.Nome);
            Assert.Equal("ong@teste.com", resultado.Email);
        }

        [Fact]
        public async Task GetOngAsync_QuandoOngNaoExiste_DeveLancarExcecao()
        {
            // Arrange
            _ongRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Ong)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ErroNaoEncontrado>(
                () => _ongService.GetOngAsync(1));
            Assert.Equal("ONG com ID 1 não foi encontrada", exception.Message);
        }

        [Fact]
        public async Task GetVoluntariosDisponiveisAsync_QuandoOngNaoExiste_DeveLancarExcecao()
        {
            // Arrange
            _ongRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Ong)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ErroNaoEncontrado>(
                () => _ongService.GetVoluntariosDisponiveisAsync(1, 1, 10));
            Assert.Equal("ONG com ID 1 não foi encontrada", exception.Message);
        }

        [Fact]
        public async Task GetVoluntariosDisponiveisAsync_QuandoOngExiste_DeveRetornarVoluntariosDisponiveis()
        {
            // Arrange
            var ong = new Ong
            {
                Id = 1,
                Nome = "Teste ONG",
                AreasAtuacao = new[] { AreaAtuacao.EducacaoEnsino }
            };

            var usuarios = new List<Usuario>
            {
                new Usuario
                {
                    Id = 1,
                    Nome = "Teste Usuario",
                    Email = "usuario@teste.com",
                    Telefone = "123456789",
                    Disponivel = true,
                    AreasInteresse = new[] { AreaAtuacao.EducacaoEnsino }
                }
            };

            _ongRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(ong);
            _usuarioRepositoryMock.Setup(r => r.GetDisponiveisByAreasAsync(
                    It.Is<IEnumerable<AreaAtuacao>>(areas => areas.Contains(AreaAtuacao.EducacaoEnsino)),
                    0, 10))
                .ReturnsAsync(usuarios);
            _usuarioRepositoryMock.Setup(r => r.GetDisponiveisByAreasCountAsync(
                    It.Is<IEnumerable<AreaAtuacao>>(areas => areas.Contains(AreaAtuacao.EducacaoEnsino))))
                .ReturnsAsync(1);

            // Act
            var resultado = await _ongService.GetVoluntariosDisponiveisAsync(1, 1, 10);

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
            Assert.Equal("usuario@teste.com", usuario.Email);
            Assert.True(usuario.Disponivel);
        }
    }
} 