using Microsoft.EntityFrameworkCore;
using RendaFixa.Domain.Entities;
using RendaFixa.Domain.Interfaces;
using RendaFixa.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RendaFixa.Infrastructure.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly RendaFixaDBContext _context;

        public ContaRepository(RendaFixaDBContext context)
        {
            _context = context;
        }

        public async Task<List<ContaInvestidor>> ObterTodas()
        {
            return await _context.Contas.ToListAsync();
        }

        public async Task<ContaInvestidor> ObterPorId(int id)
        {
            return await _context.Contas.FindAsync(id);
        }

        public async Task Adicionar(ContaInvestidor conta)
        {
            await _context.Contas.AddAsync(conta);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(ContaInvestidor conta)
        {
            _context.Contas.Update(conta);
            await _context.SaveChangesAsync();
        }

        public async Task Remover(int id)
        {
            var conta = await _context.Contas.FindAsync(id);
            if (conta != null)
            {
                _context.Contas.Remove(conta);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ContaInvestidor> ObterConta()
        {
            return await _context.Contas.FindAsync();
        }

        public Task AtualizarConta(ContaInvestidor conta)
        {
            _context.Entry(conta).State = EntityState.Modified;
            _context.Contas.Update(conta);
           return _context.SaveChangesAsync();
        }
    }
}
