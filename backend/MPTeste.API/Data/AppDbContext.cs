using Microsoft.EntityFrameworkCore;
using MPTeste.API.Models;

namespace MPTeste.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transacao>()
                .HasIndex(t => t.PessoaId);

            modelBuilder.Entity<Transacao>()
                .HasIndex(t => t.CategoriaId);

            // Configuração para garantir que ao deletar uma Pessoa,
            // as transações dela sejam deletadas Cascade
            modelBuilder.Entity<Transacao>()
                .HasOne(t => t.Pessoa)
                .WithMany(p => p.Transacoes)
                .HasForeignKey(t => t.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração para garantir que ao deletar uma Categoria
            // Evita apagar categoria que já tem transações (Restrict)
            modelBuilder.Entity<Transacao>()
                .HasOne(t => t.Categoria)
                .WithMany()
                .HasForeignKey(t => t.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}