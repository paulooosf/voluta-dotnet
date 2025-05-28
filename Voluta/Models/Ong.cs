using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public string AreaAtuacao { get; set; }

        public string Endereco { get; set; }

        public DateTime DataCadastro { get; set; }

        // Relacionamento com Usuários (voluntários)
        public virtual ICollection<Usuario> Voluntarios { get; set; }
    }
} 