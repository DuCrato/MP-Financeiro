using Microsoft.EntityFrameworkCore;
using MPTeste.API.Data;
using MPTeste.API.DTOs;
using MPTeste.API.Models;

namespace MPTeste.API.Services
{
    /// <summary>
    /// Serviço responsável pelas regras de negócio de Categorias
    /// </summary>
    public class CategoriaService(AppDbContext context)
    {
        /// <summary>
        /// Lista todas as categorias cadastradas no banco de dados
        /// </summary>
        public async Task<List<CategoriaResponseDto>> ListarAsync()
        {
            return await context.Categorias
                .Select(c => new CategoriaResponseDto
                {
                    Id = c.Id,
                    Descricao = c.Descricao,
                    Finalidade = c.Finalidade
                })
                .ToListAsync();
        }

        /// <summary>
        /// Cria uma nova categoria, validando se a descrição já existe
        /// </summary>
        /// <param name="request">Dados da nova categoria</param>
        /// <returns>A categoria criada.</returns>
        /// <exception cref="Exception">Lançada se já existir uma categoria com a mesma descrição</exception>
        public async Task<CategoriaResponseDto> CriarAsync(CategoriaRequestDto request)
        {
            var descricao = request.Descricao.Trim();

            bool existe = await context.Categorias
                .AnyAsync(c => c.Descricao == descricao);

            if (existe)
                throw new Exception("Já existe uma categoria com essa descrição.");

            var categoria = new Categoria
            {
                Descricao = descricao,
                Finalidade = request.Finalidade
            };

            context.Categorias.Add(categoria);
            await context.SaveChangesAsync();

            return new CategoriaResponseDto
            {
                Id = categoria.Id,
                Descricao = categoria.Descricao,
                Finalidade = categoria.Finalidade
            };
        }
    }

}
