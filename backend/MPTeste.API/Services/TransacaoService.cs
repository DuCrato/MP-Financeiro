using Microsoft.EntityFrameworkCore;
using MPTeste.API.Data;
using MPTeste.API.DTOs;
using MPTeste.API.Enums;
using MPTeste.API.Exceptions;
using MPTeste.API.Models;

namespace MPTeste.API.Services
{
    /// <summary>
    /// Serviço responsável pelo cadastro e validação de transações financeiras.
    /// </summary>
    public class TransacaoService(AppDbContext context, ILogger<TransacaoService> logger)
    {
        /// <summary>
        /// Lista todas as transações, trazendo os dados relacionados de Pessoa e Categoria.
        /// </summary>
        public async Task<List<TransacaoResponseDto>> ListarAsync()
        {
            logger.LogInformation("Listando todas as transações");

            return await context.Transacoes
                .AsNoTracking()
                .Include(t => t.Pessoa)
                .Include(t => t.Categoria)
                .Select(t => new TransacaoResponseDto
                {
                    Id = t.Id,
                    Descricao = t.Descricao,
                    Valor = t.Valor,
                    Tipo = t.Tipo,
                    PessoaId = t.PessoaId,
                    PessoaNome = t.Pessoa != null ? t.Pessoa.Nome : "Desconhecido",
                    CategoriaId = t.CategoriaId,
                    CategoriaDescricao = t.Categoria != null ? t.Categoria.Descricao : "Desconhecido"
                })
                .ToListAsync();
        }

        /// <summary>
        /// Cria uma nova transação aplicando validações de negócio.
        /// </summary>
        /// <remarks>
        /// Regras aplicadas:
        /// 1. Menores de idade não podem ter Receitas, apenas Despesas.
        /// 2. O tipo da transação deve corresponder à finalidade da categoria.
        /// </remarks>
        /// <exception cref="NotFoundException">Lançada em caso de IDs inválidos.</exception>
        /// <exception cref="BusinessRuleViolationException">Lançada em caso de violação das regras de negócio.</exception>
        public async Task<TransacaoResponseDto> CriarAsync(TransacaoRequestDto request)
        {
            if (request.PessoaId <= 0)
                throw new ValidationException("ID da pessoa deve ser um número positivo.");

            if (request.CategoriaId <= 0)
                throw new ValidationException("ID da categoria deve ser um número positivo.");

            var pessoa = await context.Pessoas.FindAsync(request.PessoaId)
                ?? throw new NotFoundException($"Pessoa com ID {request.PessoaId} não encontrada.");

            var categoria = await context.Categorias.FindAsync(request.CategoriaId)
                ?? throw new NotFoundException($"Categoria com ID {request.CategoriaId} não encontrada.");

            ValidarRegrasDeNegocio(pessoa, categoria, request.Tipo);

            logger.LogInformation("Criando transação: {Descricao}, Valor: {Valor}, Tipo: {Tipo}", 
                request.Descricao, request.Valor, request.Tipo);

            var transacao = new Transacao
            {
                Descricao = request.Descricao,
                Valor = request.Valor,
                Tipo = request.Tipo,
                PessoaId = request.PessoaId,
                CategoriaId = request.CategoriaId
            };

            context.Transacoes.Add(transacao);
            await context.SaveChangesAsync();

            logger.LogInformation("Transação criada com sucesso. ID: {Id}", transacao.Id);

            return new TransacaoResponseDto
            {
                Id = transacao.Id,
                Descricao = transacao.Descricao,
                Valor = transacao.Valor,
                Tipo = transacao.Tipo,
                PessoaId = pessoa.Id,
                PessoaNome = pessoa.Nome,
                CategoriaId = categoria.Id,
                CategoriaDescricao = categoria.Descricao
            };
        }

        /// <summary>
        /// Valida as regras de negócio para a criação de uma transação.
        /// Lança uma exceção caso alguma regra seja violada.
        /// </summary>
        /// <param name="pessoa">Dados da pessoa.</param>
        /// <param name="categoria">Dados da categoria.</param>
        /// <param name="tipoTransacao">Tipo da transação (Receita/Despesa).</param>
        /// <exception cref="BusinessRuleViolationException">Lançada quando uma regra de negócio impede a operação.</exception>
        private static void ValidarRegrasDeNegocio(Pessoa pessoa, Categoria categoria, TipoTransacao tipoTransacao)
        {
            // Regra 1: Menores de idade não podem ter receitas
            if (pessoa.Idade < 18 && tipoTransacao == TipoTransacao.Receita)
                throw new BusinessRuleViolationException("Menores de idade só podem cadastrar despesas.");

            // Regra 2: Tipo da transação incompatível com a Categoria
            if (tipoTransacao == TipoTransacao.Receita && categoria.Finalidade == FinalidadeCategoria.Despesa)
                throw new BusinessRuleViolationException("Categoria inválida para receita.");

            if (tipoTransacao == TipoTransacao.Despesa && categoria.Finalidade == FinalidadeCategoria.Receita)
                throw new BusinessRuleViolationException("Categoria inválida para despesa.");
        }
    }
}