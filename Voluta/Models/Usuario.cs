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

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "O email informado não é válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [RegularExpression(@"^\(\d{2}\) \d{5}-\d{4}$", ErrorMessage = "O telefone deve estar no formato (99) 99999-9999")]
        public string Telefone { get; set; }

        public bool Disponivel { get; set; }

        [Required(ErrorMessage = "A data de cadastro é obrigatória")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "As áreas de interesse são obrigatórias")]
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