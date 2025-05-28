using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voluta.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Telefone { get; set; }

        public bool Disponivel { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Required]
        public string AreasInteresseJson { get; set; }

        [NotMapped]
        public AreaAtuacao[] AreasInteresse
        {
            get => string.IsNullOrEmpty(AreasInteresseJson) 
                ? Array.Empty<AreaAtuacao>() 
                : System.Text.Json.JsonSerializer.Deserialize<AreaAtuacao[]>(AreasInteresseJson);
            set => AreasInteresseJson = System.Text.Json.JsonSerializer.Serialize(value);
        }

        public virtual ICollection<Ong> OngsVoluntario { get; set; }
        public virtual ICollection<SolicitacaoVoluntariado> SolicitacoesVoluntariado { get; set; }
    }
} 