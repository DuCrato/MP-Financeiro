using Microsoft.AspNetCore.Mvc;
using MPTeste.API.DTOs;
using MPTeste.API.Services;

namespace MPTeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController(CategoriaService service) : ControllerBase
    {
        /// <summary>
        /// Lista todas as categorias cadastradas.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaResponseDto>>> BuscarCategorias()
        {
            var categorias = await service.ListarAsync();

            return Ok(categorias);
        }

        /// <summary>
        /// Cadastra uma nova categoria (Despesa, Receita ou Ambas).
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CategoriaResponseDto>> CriarCategoria(
            [FromBody] CategoriaRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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