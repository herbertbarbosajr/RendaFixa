namespace RendaFixa.Application.DTO_s
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Indexador { get; set; } = string.Empty;
        public decimal PrecoUnitario { get; set; }
        public decimal Taxa { get; set; }
        public int Estoque { get; set; }
    }
}
