using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Voluta.ViewModels
{
    public class NovoUsuarioViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [StringLength(256, ErrorMessage = "O e-mail deve ter no máximo 256 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres")]
        public string Telefone { get; set; }

        public bool Disponivel { get; set; } = true;

        [Required(ErrorMessage = "As áreas de interesse são obrigatórias")]
        public List<string> AreasInteresse { get; set; }
    }
} 