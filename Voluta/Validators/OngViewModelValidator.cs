using FluentValidation;
using Voluta.ViewModels;

namespace Voluta.Validators
{
    public class OngViewModelValidator : AbstractValidator<OngViewModel>
    {
        public OngViewModelValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .Length(2, 100).WithMessage("O nome deve ter entre 2 e 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório")
                .EmailAddress().WithMessage("O email informado não é válido")
                .MaximumLength(100).WithMessage("O email deve ter no máximo 100 caracteres");

            RuleFor(x => x.Telefone)
                .NotEmpty().WithMessage("O telefone é obrigatório")
                .Matches(@"^\(\d{2}\) \d{5}-\d{4}$").WithMessage("O telefone deve estar no formato (99) 99999-9999");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória")
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres");

            RuleFor(x => x.AreasAtuacao)
                .NotEmpty().WithMessage("Pelo menos uma área de atuação deve ser selecionada");

            RuleFor(x => x.Endereco)
                .NotEmpty().WithMessage("O endereço é obrigatório")
                .MaximumLength(200).WithMessage("O endereço deve ter no máximo 200 caracteres");
        }
    }
} 