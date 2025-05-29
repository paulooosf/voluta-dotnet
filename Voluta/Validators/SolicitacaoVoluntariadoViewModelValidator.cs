using FluentValidation;
using Voluta.ViewModels;
using Voluta.Models;

namespace Voluta.Validators
{
    public class SolicitacaoVoluntariadoViewModelValidator : AbstractValidator<SolicitacaoVoluntariadoViewModel>
    {
        public SolicitacaoVoluntariadoViewModelValidator()
        {
            RuleFor(x => x.UsuarioId)
                .GreaterThan(0).WithMessage("O ID do usuário é inválido");

            RuleFor(x => x.OngId)
                .GreaterThan(0).WithMessage("O ID da ONG é inválido");

            RuleFor(x => x.DataSolicitacao)
                .NotEmpty().WithMessage("A data da solicitação é obrigatória")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data da solicitação não pode ser futura");

            RuleFor(x => x.DataAprovacao)
                .Must((solicitacao, dataAprovacao) =>
                {
                    if (solicitacao.Status == StatusSolicitacao.Aprovada && !dataAprovacao.HasValue)
                        return false;
                    if (dataAprovacao.HasValue && dataAprovacao.Value < solicitacao.DataSolicitacao)
                        return false;
                    return true;
                })
                .WithMessage("A data de aprovação deve ser posterior à data da solicitação");

            RuleFor(x => x.Mensagem)
                .MaximumLength(500).WithMessage("A mensagem deve ter no máximo 500 caracteres");
        }
    }

    public class NovaSolicitacaoVoluntariadoViewModelValidator : AbstractValidator<NovaSolicitacaoVoluntariadoViewModel>
    {
        public NovaSolicitacaoVoluntariadoViewModelValidator()
        {
            RuleFor(x => x.OngId)
                .GreaterThan(0).WithMessage("O ID da ONG é inválido");

            RuleFor(x => x.Mensagem)
                .NotEmpty().WithMessage("A mensagem é obrigatória")
                .MaximumLength(500).WithMessage("A mensagem deve ter no máximo 500 caracteres");
        }
    }
} 