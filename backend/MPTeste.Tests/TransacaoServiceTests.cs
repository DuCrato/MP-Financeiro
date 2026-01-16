using Microsoft.EntityFrameworkCore;
using MPTeste.API.Data;
using MPTeste.API.DTOs;
using MPTeste.API.Enums;
using MPTeste.API.Models;
using MPTeste.API.Services;

namespace MPTeste.Tests
{
    public class TransacaoServiceTests
    {
        // Método auxiliar para criar um banco de dados "fake" limpo para cada teste
        private AppDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nome único para não misturar testes
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task CriarAsync_MenorDeIdadeTentandoReceita_DeveLancarException()
        {
            // 1. Arrange (Preparação)
            var context = GetDatabaseContext();

            // Cria dados falsos no banco em memória
            var pessoa = new Pessoa { Nome = "Menor Aprendiz", Idade = 16 };
            var categoria = new Categoria { Descricao = "Salário", Finalidade = FinalidadeCategoria.Receita };

            context.Pessoas.Add(pessoa);
            context.Categorias.Add(categoria);
            await context.SaveChangesAsync();

            var service = new TransacaoService(context);

            var request = new TransacaoRequestDto
            {
                PessoaId = pessoa.Id,
                CategoriaId = categoria.Id,
                Valor = 500,
                Tipo = TipoTransacao.Receita, // ERRO: Menor não pode ter receita
                Descricao = "Teste Erro"
            };

            // 2. Act & Assert (Ação e Verificação)
            // Esperamos que o serviço lance InvalidOperationException
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CriarAsync(request));

            Assert.Equal("Menores de idade só podem cadastrar despesas.", exception.Message);
        }

        [Fact]
        public async Task CriarAsync_CategoriaErradaParaDespesa_DeveLancarException()
        {
            // 1. Arrange
            var context = GetDatabaseContext();

            var pessoa = new Pessoa { Nome = "Adulto", Idade = 30 };
            // Categoria é de RECEITA
            var categoria = new Categoria { Descricao = "Salário", Finalidade = FinalidadeCategoria.Receita };

            context.Pessoas.Add(pessoa);
            context.Categorias.Add(categoria);
            await context.SaveChangesAsync();

            var service = new TransacaoService(context);

            var request = new TransacaoRequestDto
            {
                PessoaId = pessoa.Id,
                CategoriaId = categoria.Id,
                Valor = 100,
                Tipo = TipoTransacao.Despesa, // ERRO: Tentando lançar despesa em categoria de receita
                Descricao = "Erro Categoria"
            };

            // 2. Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CriarAsync(request));

            Assert.Equal("Categoria inválida para despesa.", exception.Message);
        }

        [Fact]
        public async Task CriarAsync_DadosValidos_DeveCriarTransacao()
        {
            // 1. Arrange
            var context = GetDatabaseContext();

            var pessoa = new Pessoa { Nome = "João", Idade = 30 };
            var categoria = new Categoria { Descricao = "Alimentação", Finalidade = FinalidadeCategoria.Despesa };

            context.Pessoas.Add(pessoa);
            context.Categorias.Add(categoria);
            await context.SaveChangesAsync();

            var service = new TransacaoService(context);
            var request = new TransacaoRequestDto
            {
                PessoaId = pessoa.Id,
                CategoriaId = categoria.Id,
                Valor = 50,
                Tipo = TipoTransacao.Despesa,
                Descricao = "Almoço"
            };

            // 2. Act
            var resultado = await service.CriarAsync(request);

            // 3. Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Id > 0); // Verifica se gerou ID
            Assert.Equal("Almoço", resultado.Descricao);

            // Verifica se salvou no banco mesmo
            var transacaoNoBanco = await context.Transacoes.FirstOrDefaultAsync(t => t.Id == resultado.Id);
            Assert.NotNull(transacaoNoBanco);
        }
    }
}