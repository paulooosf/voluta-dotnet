using System;
using System.Collections.Generic;
using Voluta.Models;

namespace Voluta.ViewModels
{
    public class OngViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Descricao { get; set; }
        public AreaAtuacao[] AreasAtuacao { get; set; }
        public string Endereco { get; set; }
        public DateTime DataCadastro { get; set; }

        public static OngViewModel FromOng(Ong ong)
        {
            return new OngViewModel
            {
                Id = ong.Id,
                Nome = ong.Nome,
                Email = ong.Email,
                Telefone = ong.Telefone,
                Descricao = ong.Descricao,
                AreasAtuacao = ong.AreasAtuacao,
                Endereco = ong.Endereco,
                DataCadastro = ong.DataCadastro
            };
        }
    }
} 