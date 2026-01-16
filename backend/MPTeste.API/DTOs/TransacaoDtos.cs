using System.ComponentModel.DataAnnotations;
using MPTeste.API.Enums;

namespace MPTeste.API.DTOs
{
    // Como o arquivo é pequeno, coloquei os dois DTOs juntos aqui. Mas poderiam ser separados
    // sendo um arquivo para TransacaoRequestDto.cs e outro para TransacaoResponseDto.cs
    
    // DTO de Entrada (Request) - O que o front manda para criar
    public class TransacaoRequestDto
    {
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [Required]
        public TipoTransacao Tipo { get; set; }

        [Required]
        public int PessoaId { get; set; }

        [Required]
        public int CategoriaId { get; set; }
    }

    // DTO de Saída (Response) - O que a API devolve para o front ler
    public class TransacaoResponseDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public TipoTransacao Tipo { get; set; }
        public string TipoDescricao => Tipo.ToString();

        // Dados achatados (Flattened) para facilitar a tabela no Front
        public int PessoaId { get; set; }
        public string PessoaNome { get; set; } = string.Empty;

        public int CategoriaId { get; set; }
        public string CategoriaDescricao { get; set; } = string.Empty;
    }
}