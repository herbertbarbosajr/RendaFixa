using Microsoft.EntityFrameworkCore;
using FixedIncome.Domain.Entities;
using FixedIncome.Domain.Interfaces;
using FixedIncome.Infrastructure.Data;

namespace FixedIncome.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FixedIncomeDBContext _context;

        public AccountRepository(FixedIncomeDBContext context)
        {
            _context = context;
        }

        public async Task<List<InvestorAccount>> GetAll()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<InvestorAccount> GetById(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task ToAdd(InvestorAccount conta)
        {
            await _context.Accounts.AddAsync(conta);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccount(InvestorAccount conta)
        {
            _context.Accounts.Update(conta);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(int id)
        {
            var conta = await _context.Accounts.FindAsync(id);
            if (conta != null)
            {
                _context.Accounts.Remove(conta);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<InvestorAccount> GetAccount()
        {
            return await _context.Accounts.FindAsync();
        }
    }
}
