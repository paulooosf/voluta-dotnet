using System;
using Voluta.Models;

namespace Voluta.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public bool Disponivel { get; set; }
        public AreaAtuacao[] AreasInteresse { get; set; }
        public DateTime DataCadastro { get; set; }

        public static UsuarioViewModel FromUsuario(Usuario usuario)
        {
            return new UsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                Disponivel = usuario.Disponivel,
                AreasInteresse = usuario.AreasInteresse,
                DataCadastro = usuario.DataCadastro
            };
        }
    }
} 