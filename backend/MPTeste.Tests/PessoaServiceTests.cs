using Microsoft.EntityFrameworkCore;
using MPTeste.API.Data;
using MPTeste.API.DTOs;
using MPTeste.API.Enums;
using MPTeste.API.Models;
using MPTeste.API.Services;

namespace MPTeste.Tests
{
    public class PessoaServiceTests
    {
        private AppDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task CriarAsync_DeveSalvarPessoaNoBanco()
        {
            // Arrange
            var context = GetDatabaseContext();
            var service = new PessoaService(context);
            var request = new PessoaRequestDto { Nome = "Maria", Idade = 25 };

            // Act
            var result = await service.CriarAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);

            var pessoaNoBanco = await context.Pessoas.FindAsync(result.Id);
            Assert.Equal("Maria", pessoaNoBanco.Nome);
        }

        [Fact]
        public async Task ObterRelatorioAsync_DeveCalcularTotaisESaldoCorretamente()
        {
            // Arrange
            var context = GetDatabaseContext();
            var service = new PessoaService(context);

            // 1. Criar Pessoa
            var pessoa = new Pessoa { Nome = "Investidor", Idade = 30 };
            context.Pessoas.Add(pessoa);

            // 2. Criar Transações (Simulando dados no banco)
            // Receita: 1000
            // Despesa: 200
            // Saldo esperado: 800
            context.Transacoes.AddRange(
                new Transacao { Pessoa = pessoa, Valor = 1000, Tipo = TipoTransacao.Receita },
                new Transacao { Pessoa = pessoa, Valor = 200, Tipo = TipoTransacao.Despesa }
            );
            await context.SaveChangesAsync();

            // Act
            var relatorio = await service.ObterRelatorioAsync();
            var dadosPessoa = relatorio.Pessoas.First(p => p.Id == pessoa.Id);

            // Assert
            Assert.Equal(1000, dadosPessoa.TotalReceitas);
            Assert.Equal(200, dadosPessoa.TotalDespesas);
            Assert.Equal(800, dadosPessoa.Saldo); // 1000 - 200
        }
    }
}