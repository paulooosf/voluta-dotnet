using System;
using Voluta.Models;

namespace Voluta.ViewModels
{
    public class SolicitacaoVoluntariadoViewModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public int OngId { get; set; }
        public string NomeOng { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public StatusSolicitacao Status { get; set; }
        public string Mensagem { get; set; }

        public static SolicitacaoVoluntariadoViewModel FromSolicitacao(SolicitacaoVoluntariado solicitacao)
        {
            return new SolicitacaoVoluntariadoViewModel
            {
                Id = solicitacao.Id,
                UsuarioId = solicitacao.UsuarioId,
                NomeUsuario = solicitacao.Usuario?.Nome,
                OngId = solicitacao.OngId,
                NomeOng = solicitacao.Ong?.Nome,
                DataSolicitacao = solicitacao.DataSolicitacao,
                DataAprovacao = solicitacao.DataAprovacao,
                Status = solicitacao.Status,
                Mensagem = solicitacao.Mensagem
            };
        }
    }

    public class NovaSolicitacaoVoluntariadoViewModel
    {
        public int OngId { get; set; }
        public string Mensagem { get; set; }
    }
} 