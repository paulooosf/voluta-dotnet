using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Voluta.ViewModels
{
    public class AtualizarUsuarioViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres")]
        public string Telefone { get; set; }

        public bool Disponivel { get; set; }

        [Required(ErrorMessage = "As áreas de interesse são obrigatórias")]
        public List<string> AreasInteresse { get; set; }
    }
} 