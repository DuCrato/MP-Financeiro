using Microsoft.AspNetCore.Mvc;
using MPTeste.API.DTOs;
using MPTeste.API.Services;

namespace MPTeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController(PessoaService service) : ControllerBase
    {
        /// <summary>
        /// Lista todas as pessoas cadastradas.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaResponseDto>>> BuscarPessoas()
        {
            var pessoas = await service.ListarAsync();

            return Ok(pessoas);
        }

        /// <summary>
        /// Retorna o relatório financeiro com totais por pessoa e geral.
        /// </summary>
        [HttpGet("totais")]
        public async Task<ActionResult<RelatorioPessoasDto>> GetTotais()
        {
            var relatorio = await service.ObterRelatorioAsync();

            return Ok(relatorio);
        }

        /// <summary>
        /// Cadastra uma nova pessoa.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PessoaResponseDto>> CriarPessoa(
            [FromBody] PessoaRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await service.CriarAsync(request);

            return Created(string.Empty, response);
        }

        /// <summary>
        /// Remove uma pessoa e todas as suas transações.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPessoa(int id)
        {
            try
            {
                await service.ExcluirAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}