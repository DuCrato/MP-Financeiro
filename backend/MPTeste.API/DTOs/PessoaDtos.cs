using System.ComponentModel.DataAnnotations;

namespace MPTeste.API.DTOs
{
    // Como o arquivo é pequeno, coloquei os dois DTOs juntos aqui. Mas poderiam ser separados
    // sendo um arquivo para PessoaRequestDto.cs e outro para PessoaResponseDto.cs

    // DTO de Entrada (Request) - O que o front manda para criar
    public class PessoaRequestDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [Range(1, 100, ErrorMessage = "A idade deve ser um número positivo válido.")]
        public int Idade { get; set; }
    }

    // DTO de Saída (Response) - O que a API devolve para o front ler
    public class PessoaResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
    }
}