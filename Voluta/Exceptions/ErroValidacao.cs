using FluentValidation.Results;

namespace Voluta.Exceptions
{
    public class ErroValidacao : Exception
    {
        public IEnumerable<string> Erros { get; }

        public ErroValidacao() : base("Um ou mais erros de validação ocorreram.")
        {
            Erros = new List<string>();
        }

        public ErroValidacao(IEnumerable<ValidationFailure> falhas) : this()
        {
            Erros = falhas.Select(f => f.ErrorMessage);
        }

        public ErroValidacao(IEnumerable<string> erros) : this()
        {
            Erros = erros;
        }
    }
} 