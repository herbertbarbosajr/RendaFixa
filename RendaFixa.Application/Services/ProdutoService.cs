using MassTransit;
using RendaFixa.Application.DTO_s;
using RendaFixa.Application.Interfaces;
using RendaFixa.Domain.Interfaces;
using RendaFixa.Infrastructure.Events;

namespace RendaFixa.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepo;
        private readonly IContaRepository _contaRepo;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProdutoService(IProdutoRepository produtoRepo, IContaRepository contaRepo, IPublishEndpoint publishEndpoint)
        {
            _produtoRepo = produtoRepo;
            _contaRepo = contaRepo;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<List<ProdutoDto>> ObterProdutosOrdenados()
        {
            var produtos = await _produtoRepo.ObterTodos();
            return [.. produtos.OrderByDescending(p => p.Taxa).Select(p => new ProdutoDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Indexador = p.Indexador,
                PrecoUnitario = p.PrecoUnitario,
                Taxa = p.Taxa,
                Estoque = p.Estoque
            })];
        }

        public async Task RealizarCompra(int produtoId, int quantidade)
        {
            var produtos = await _produtoRepo.ObterTodos();
            var produto = produtos.FirstOrDefault(p => p.Id == produtoId)
                          ?? throw new Exception("Produto não encontrado");

            var conta = await _contaRepo.ObterConta();

            produto.DebitarEstoque(quantidade);
            conta.DebitarSaldo(produto.PrecoUnitario * quantidade);

            await _produtoRepo.AtualizarProdutos(produtos);
            await _contaRepo.AtualizarConta(conta);
            await _publishEndpoint.Publish(new CompraRealizadaEvent(produtoId, quantidade, produto.PrecoUnitario * quantidade, DateTime.UtcNow));
        }

        public async Task<decimal> ObterSaldo(int id)
        {
            var conta = await _contaRepo.ObterPorId(id);
            return conta.Saldo;
        }
    }
}
