using Microsoft.AspNetCore.Mvc;
using MPTeste.API.DTOs;
using MPTeste.API.Services;

namespace MPTeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransacoesController(TransacaoService service) : ControllerBase
    {
        /// <summary>
        /// Lista todas as transações, incluindo dados da Pessoa e Categoria.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransacaoResponseDto>>> BuscarTransacoes()
        {
            var transacoes = await service.ListarAsync();

            return Ok(transacoes);
        }

        /// <summary>
        /// Cria uma nova transação aplicando as regras de negócio.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TransacaoResponseDto>> CriarTransacao(TransacaoRequestDto request)
        {
            try
            {
                var response = await service.CriarAsync(request);

                return Created(string.Empty, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}