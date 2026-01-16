using Microsoft.EntityFrameworkCore;
using MPTeste.API.Data;
using MPTeste.API.DTOs;
using MPTeste.API.Models;

namespace MPTeste.API.Services
{
    /// <summary>
    /// Gerencia as operações referentes a Pessoas e seus relatórios financeiros
    /// </summary>
    public class PessoaService(AppDbContext context)
    {
        /// <summary>
        /// Retorna a lista simples de pessoas
        /// </summary>
        public async Task<List<PessoaResponseDto>> ListarAsync()
        {
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
            var pessoa = new Pessoa
            {
                Nome = request.Nome,
                Idade = request.Idade
            };

            context.Pessoas.Add(pessoa);
            await context.SaveChangesAsync();

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
        /// <exception cref="Exception">Lançada caso o ID não exista</exception>
        public async Task ExcluirAsync(int id)
        {
            var pessoa = await context.Pessoas.FindAsync(id)
                ?? throw new Exception("Pessoa não encontrada.");

            context.Pessoas.Remove(pessoa);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Gera um relatório consolidado com totais de Receitas, Despesas e Saldo por pessoa
        /// </summary>
        /// <returns>Objeto contendo a lista detalhada e os totais gerais</returns>
        public async Task<RelatorioPessoasDto> ObterRelatorioAsync()
        {
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

            return new RelatorioPessoasDto
            {
                Pessoas = pessoas,
                TotalGeralReceitas = pessoas.Sum(p => p.TotalReceitas),
                TotalGeralDespesas = pessoas.Sum(p => p.TotalDespesas),
                SaldoGeralLiquido = pessoas.Sum(p => p.Saldo)
            };
        }
    }

}
