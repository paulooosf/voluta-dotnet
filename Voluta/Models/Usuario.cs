using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public DateTime DataCadastro { get; set; }

        public virtual ICollection<Ong> OngsVoluntario { get; set; }
    }
} 