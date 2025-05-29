using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voluta.Models
{
    public class SolicitacaoVoluntariado
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O ID do usuário é obrigatório")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "O ID da ONG é obrigatório")]
        public int OngId { get; set; }

        [Required(ErrorMessage = "A data da solicitação é obrigatória")]
        public DateTime DataSolicitacao { get; set; } = DateTime.Now;

        public DateTime? DataAprovacao { get; set; }

        [Required(ErrorMessage = "O status é obrigatório")]
        public StatusSolicitacao Status { get; set; } = StatusSolicitacao.Pendente;

        [Required(ErrorMessage = "A mensagem é obrigatória")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "A mensagem deve ter entre 10 e 500 caracteres")]
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