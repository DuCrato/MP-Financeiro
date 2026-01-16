using System.ComponentModel.DataAnnotations;
using MPTeste.API.Enums;

namespace MPTeste.API.DTOs
{
    // Como o arquivo é pequeno, coloquei os dois DTOs juntos aqui. Mas poderiam ser separados
    // sendo um arquivo para CategoriaRequestDto.cs e outro para CategoriaResponseDto.cs
    
    // DTO de Entrada (Request) - O que o front manda para criar
    public class CategoriaRequestDto
    {
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [MaxLength(100, ErrorMessage = "A descrição deve ter no máximo 100 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A finalidade é obrigatória.")]
        public FinalidadeCategoria Finalidade { get; set; }
    }

    // DTO de Saída (Response) - O que a API devolve para o front ler
    public class CategoriaResponseDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        
        public FinalidadeCategoria Finalidade { get; set; }
        
        // Propriedade extra para facilitar a vida do front
        public string FinalidadeDescricao => Finalidade.ToString(); 
    }
}