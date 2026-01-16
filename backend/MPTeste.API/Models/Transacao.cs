using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MPTeste.API.Enums;

namespace MPTeste.API.Models
{
    /// <summary>
    /// Representa uma movimentação financeira (Receita ou Despesa).
    /// </summary>
    public class Transacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser positivo.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required]
        public TipoTransacao Tipo { get; set; }

        [Required]
        public int PessoaId { get; set; }
        public Pessoa Pessoa { get; set; } = null!;

        [Required]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;
    }
}