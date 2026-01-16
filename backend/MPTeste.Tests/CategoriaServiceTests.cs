using Microsoft.EntityFrameworkCore;
using MPTeste.API.Data;
using MPTeste.API.DTOs;
using MPTeste.API.Enums;
using MPTeste.API.Models;
using MPTeste.API.Services;

namespace MPTeste.Tests
{
    public class CategoriaServiceTests
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
        public async Task CriarAsync_DescricaoDuplicada_DeveLancarException()
        {
            // Arrange
            var context = GetDatabaseContext();

            // Já existe "Alimentação" no banco
            context.Categorias.Add(new Categoria { Descricao = "Alimentação", Finalidade = FinalidadeCategoria.Despesa });
            await context.SaveChangesAsync();

            var service = new CategoriaService(context);
            var request = new CategoriaRequestDto { Descricao = "Alimentação", Finalidade = FinalidadeCategoria.Despesa };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => service.CriarAsync(request));
            Assert.Equal("Já existe uma categoria com essa descrição.", ex.Message);
        }

        [Fact]
        public async Task CriarAsync_NovaCategoria_DeveSalvarComSucesso()
        {
            // Arrange
            var context = GetDatabaseContext();
            var service = new CategoriaService(context);
            var request = new CategoriaRequestDto { Descricao = "Lazer", Finalidade = FinalidadeCategoria.Despesa };

            // Act
            var result = await service.CriarAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Lazer", result.Descricao);

            // Verifica se está no banco
            Assert.True(await context.Categorias.AnyAsync(c => c.Descricao == "Lazer"));
        }
    }
}