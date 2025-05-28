using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voluta.Models
{
    public class SolicitacaoVoluntariado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int OngId { get; set; }

        [Required]
        public DateTime DataSolicitacao { get; set; } = DateTime.Now;

        public DateTime? DataAprovacao { get; set; }

        [Required]
        public StatusSolicitacao Status { get; set; } = StatusSolicitacao.Pendente;

        public string Mensagem { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("OngId")]
        public virtual Ong Ong { get; set; }
    }

    public enum StatusSolicitacao
    {
        Pendente,
        Aprovada,
        Rejeitada,
        Cancelada
    }
} 