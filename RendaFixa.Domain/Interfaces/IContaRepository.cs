using RendaFixa.Domain.Entities;

namespace RendaFixa.Domain.Interfaces
{
    public interface IContaRepository
    {
        Task<ContaInvestidor> ObterConta();
        Task AtualizarConta(ContaInvestidor conta);
        Task<ContaInvestidor> ObterPorId(int id);
        Task Adicionar(ContaInvestidor conta);
        Task Atualizar(ContaInvestidor conta);
        Task Remover(int id);
        Task<List<ContaInvestidor>> ObterTodas();
    }
}
