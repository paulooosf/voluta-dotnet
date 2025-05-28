using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voluta.Models
{
    public class Ong
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(14)]
        public string Cnpj { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Telefone { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public string AreasAtuacaoJson { get; set; }

        [NotMapped]
        public AreaAtuacao[] AreasAtuacao
        {
            get => string.IsNullOrEmpty(AreasAtuacaoJson) 
                ? Array.Empty<AreaAtuacao>() 
                : System.Text.Json.JsonSerializer.Deserialize<AreaAtuacao[]>(AreasAtuacaoJson);
            set => AreasAtuacaoJson = System.Text.Json.JsonSerializer.Serialize(value);
        }

        [Required]
        public string Endereco { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public virtual ICollection<Usuario> Voluntarios { get; set; }
        public virtual ICollection<SolicitacaoVoluntariado> SolicitacoesVoluntariado { get; set; }
    }
} 