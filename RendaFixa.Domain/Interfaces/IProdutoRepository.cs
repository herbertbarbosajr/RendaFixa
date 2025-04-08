using RendaFixa.Domain.Entities;

namespace RendaFixa.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<List<ProdutoRendaFixa>> ObterTodos();
        Task AtualizarProdutos(List<ProdutoRendaFixa> produtos);
        Task Remover(int id);
        Task Adicionar(ProdutoRendaFixa produto);
        Task<ProdutoRendaFixa> ObterPorId(int id);
    }
}
