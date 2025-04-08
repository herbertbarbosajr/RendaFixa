namespace RendaFixa.Domain.Entities
{
    public class ProdutoRendaFixa
    {
        public ProdutoRendaFixa(string nome, string indexador, decimal precoUnitario, decimal taxa, int estoque)
        {
            Nome = nome;
            Indexador = indexador;
            PrecoUnitario = precoUnitario;
            Taxa = taxa;
            Estoque = estoque;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Indexador { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Taxa { get; set; }
        public int Estoque { get; set; }

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade > Estoque)
                throw new InvalidOperationException("Estoque insuficiente.");

            Estoque -= quantidade;
        }
    }
}
