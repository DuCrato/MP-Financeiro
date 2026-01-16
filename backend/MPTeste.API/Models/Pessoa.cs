using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MPTeste.API.Models
{
    /// <summary>
    /// Representa uma pessoa no sistema.
    /// </summary>
    public class Pessoa
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [Range(1, 100, ErrorMessage = "A idade deve ser um número positivo válido.")]
        public int Idade { get; set; }

        // Propriedade de navegação
        [JsonIgnore]
        public ICollection<Transacao> Transacoes { get; set; } = [];
    }
}