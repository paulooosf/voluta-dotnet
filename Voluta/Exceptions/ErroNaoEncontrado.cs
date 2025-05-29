namespace Voluta.Exceptions
{
    public class ErroNaoEncontrado : Exception
    {
        public ErroNaoEncontrado(string mensagem) : base(mensagem)
        {
        }

        public ErroNaoEncontrado(string entidade, object chave)
            : base($"{entidade} não encontrado(a) com identificador {chave}")
        {
        }
    }
} 