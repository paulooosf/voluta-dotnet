using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Voluta.Configurations;
using Voluta.Models;
using Voluta.Models.Auth;
using Voluta.Repositories;
using BC = BCrypt.Net.BCrypt;

namespace Voluta.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequest request);
        string GerarToken(Usuario usuario);
        string HashSenha(string senha);
        bool VerificarSenha(string senha, string hash);
    }

    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly JwtConfig _jwtConfig;

        public AuthService(IUsuarioRepository usuarioRepository, JwtConfig jwtConfig)
        {
            _usuarioRepository = usuarioRepository;
            _jwtConfig = jwtConfig;
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);
            if (usuario == null || !VerificarSenha(request.Senha, usuario.SenhaHash))
            {
                throw new Exception("Email ou senha inválidos");
            }

            return GerarToken(usuario);
        }

        public string GerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, "Usuario") // Você pode adicionar roles específicas aqui
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiracaoMinutos),
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string HashSenha(string senha)
        {
            return BC.HashPassword(senha);
        }

        public bool VerificarSenha(string senha, string hash)
        {
            return BC.Verify(senha, hash);
        }
    }
} 