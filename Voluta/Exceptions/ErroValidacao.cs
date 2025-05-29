using System;
using System.Collections.Generic;

namespace Voluta.Exceptions
{
    public class ErroValidacao : Exception
    {
        public IEnumerable<string> Erros { get; }

        public ErroValidacao() : base("Um ou mais erros de validação ocorreram.")
        {
            Erros = new List<string>();
        }

        public ErroValidacao(IEnumerable<string> erros) : this()
        {
            Erros = erros;
        }

        public ErroValidacao(string erro) : this()
        {
            Erros = new[] { erro };
        }
    }
} 