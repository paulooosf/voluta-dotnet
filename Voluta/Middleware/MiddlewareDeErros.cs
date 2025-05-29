using System.Net;
using System.Text.Json;
using Voluta.Exceptions;

namespace Voluta.Middleware
{
    public class MiddlewareDeErros
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareDeErros> _logger;

        public MiddlewareDeErros(RequestDelegate next, ILogger<MiddlewareDeErros> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado");
                await TratarErroAsync(context, ex);
            }
        }

        private static async Task TratarErroAsync(HttpContext context, Exception erro)
        {
            context.Response.ContentType = "application/json";
            
            // Define o status code baseado no tipo de erro
            context.Response.StatusCode = erro switch
            {
                ErroNaoEncontrado => StatusCodes.Status404NotFound,
                ErroNegocio => StatusCodes.Status400BadRequest,
                ErroValidacao validacaoEx => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var resposta = erro switch
            {
                ErroValidacao validacaoEx => new
                {
                    Mensagem = "Erro de validação",
                    Detalhes = validacaoEx.Erros
                },
                _ => new
                {
                    Mensagem = erro.Message
                }
            };

            await context.Response.WriteAsJsonAsync(resposta);
        }
    }
} 