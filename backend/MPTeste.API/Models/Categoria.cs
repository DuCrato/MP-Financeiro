using System.ComponentModel.DataAnnotations;
using MPTeste.API.Enums;

namespace MPTeste.API.Models
{
    /// <summary>
    /// Representa uma categoria de transação financeira.
    /// </summary>
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [MaxLength(100)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public FinalidadeCategoria Finalidade { get; set; }
    }
}