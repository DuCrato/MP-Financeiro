namespace MPTeste.API.DTOs
{
    // Totais de uma única pessoa
    public class PessoaTotalDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal Saldo { get; set; } // Receita - Despesa
    }

    // Objeto completo que o Front vai receber (Lista + Totais Gerais)
    public class RelatorioPessoasDto
    {
        public List<PessoaTotalDto> Pessoas { get; set; } = new();

        // Totais Gerais de TODAS as pessoas
        public decimal TotalGeralReceitas { get; set; }
        public decimal TotalGeralDespesas { get; set; }
        public decimal SaldoGeralLiquido { get; set; }
    }
}