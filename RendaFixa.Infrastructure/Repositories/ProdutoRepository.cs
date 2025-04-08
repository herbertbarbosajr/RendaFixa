using RendaFixa.Domain.Interfaces;
using RendaFixa.Domain.Entities;
using RendaFixa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RendaFixa.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly RendaFixaDBContext _context;

        public ProdutoRepository(RendaFixaDBContext context)
        {
            _context = context;
        }

        public async Task<ProdutoRendaFixa> ObterPorId(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task Adicionar(ProdutoRendaFixa produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task Remover(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }
        }

        async Task<List<ProdutoRendaFixa>> IProdutoRepository.ObterTodos()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task AtualizarProdutos(List<ProdutoRendaFixa> produtos)
        {
            foreach (var produto in produtos)
            {
                _context.Entry(produto).State = EntityState.Modified;
                _context.Produtos.Update(produto);
                _context.SaveChangesAsync();
            }
        }
    }
}
