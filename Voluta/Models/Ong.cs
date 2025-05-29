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

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter 14 dígitos")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "O CNPJ deve conter apenas números")]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "O email informado não é válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [RegularExpression(@"^\(\d{2}\) \d{5}-\d{4}$", ErrorMessage = "O telefone deve estar no formato (99) 99999-9999")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 1000 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "As áreas de atuação são obrigatórias")]
        public string AreasAtuacaoJson { get; set; }

        [NotMapped]
        public AreaAtuacao[] AreasAtuacao
        {
            get => string.IsNullOrEmpty(AreasAtuacaoJson) 
                ? Array.Empty<AreaAtuacao>() 
                : System.Text.Json.JsonSerializer.Deserialize<AreaAtuacao[]>(AreasAtuacaoJson);
            set => AreasAtuacaoJson = System.Text.Json.JsonSerializer.Serialize(value);
        }

        [Required(ErrorMessage = "O endereço é obrigatório")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "O endereço deve ter entre 5 e 200 caracteres")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "A data de cadastro é obrigatória")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public virtual ICollection<Usuario> Voluntarios { get; set; }
        public virtual ICollection<SolicitacaoVoluntariado> SolicitacoesVoluntariado { get; set; }
    }
} 