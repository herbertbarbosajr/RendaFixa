using RendaFixa.Application.DTO_s;

namespace RendaFixa.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<List<ProdutoDto>> ObterProdutosOrdenados();
        Task RealizarCompra(int produtoId, int quantidade);
        Task<decimal> ObterSaldo(int id);
    }
}
