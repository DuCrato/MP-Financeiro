using Microsoft.EntityFrameworkCore;
using MPTeste.API.Data;
using MPTeste.API.DTOs;
using MPTeste.API.Exceptions;
using MPTeste.API.Models;

namespace MPTeste.API.Services
{
    /// <summary>
    /// Gerencia as operações referentes a Pessoas e seus relatórios financeiros
    /// </summary>
    public class PessoaService(AppDbContext context, ILogger<PessoaService> logger)
    {
        /// <summary>
        /// Retorna a lista simples de pessoas
        /// </summary>
        public async Task<List<PessoaResponseDto>> ListarAsync()
        {
            logger.LogInformation("Listando todas as pessoas");

            return await context.Pessoas
                .Select(p => new PessoaResponseDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Idade = p.Idade
                })
                .ToListAsync();
        }

        /// <summary>
        /// Cadastra uma nova pessoa
        /// </summary>
        public async Task<PessoaResponseDto> CriarAsync(PessoaRequestDto request)
        {
            if (request.Idade < 0)
                throw new ValidationException("A idade não pode ser negativa.");

            logger.LogInformation("Criando nova pessoa: {Nome}, Idade: {Idade}", request.Nome, request.Idade);

            var pessoa = new Pessoa
            {
                Nome = request.Nome,
                Idade = request.Idade
            };

            context.Pessoas.Add(pessoa);
            await context.SaveChangesAsync();

            logger.LogInformation("Pessoa criada com sucesso. ID: {Id}", pessoa.Id);

            return new PessoaResponseDto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Idade = pessoa.Idade
            };
        }

        /// <summary>
        /// Remove uma pessoa do banco de dados
        /// </summary>
        /// <param name="id">ID da pessoa a ser removida</param>
        /// <exception cref="NotFoundException">Lançada caso o ID não exista</exception>
        public async Task ExcluirAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("ID deve ser um número positivo.");

            logger.LogInformation("Excluindo pessoa com ID: {Id}", id);

            var pessoa = await context.Pessoas.FindAsync(id)
                ?? throw new NotFoundException($"Pessoa com ID {id} não encontrada.");

            context.Pessoas.Remove(pessoa);
            await context.SaveChangesAsync();

            logger.LogInformation("Pessoa excluída com sucesso. ID: {Id}", id);
        }

        /// <summary>
        /// Gera um relatório consolidado com totais de Receitas, Despesas e Saldo por pessoa
        /// </summary>
        /// <returns>Objeto contendo a lista detalhada e os totais gerais</returns>
        public async Task<RelatorioPessoasDto> ObterRelatorioAsync()
        {
            logger.LogInformation("Gerando relatório de pessoas");

            // O Select aqui já calcula a soma das transações direto no banco
            var pessoas = await context.Pessoas
                .Select(p => new PessoaTotalDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    TotalReceitas = p.Transacoes
                        .Where(t => t.Tipo == Enums.TipoTransacao.Receita)
                        .Sum(t => (decimal?)t.Valor) ?? 0,
                    TotalDespesas = p.Transacoes
                        .Where(t => t.Tipo == Enums.TipoTransacao.Despesa)
                        .Sum(t => (decimal?)t.Valor) ?? 0
                })
                .ToListAsync();

            // Cálculo do saldo em memória após trazer os dados
            pessoas.ForEach(p => p.Saldo = p.TotalReceitas - p.TotalDespesas);

            var relatorio = new RelatorioPessoasDto
            {
                Pessoas = pessoas,
                TotalGeralReceitas = pessoas.Sum(p => p.TotalReceitas),
                TotalGeralDespesas = pessoas.Sum(p => p.TotalDespesas),
                SaldoGeralLiquido = pessoas.Sum(p => p.Saldo)
            };

            logger.LogInformation("Relatório gerado com sucesso. Total de pessoas: {Quantidade}", pessoas.Count);

            return relatorio;
        }
    }
}
