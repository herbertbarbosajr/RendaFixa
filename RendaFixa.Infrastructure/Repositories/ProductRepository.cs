using FixedIncome.Domain.Interfaces;
using FixedIncome.Domain.Entities;
using FixedIncome.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FixedIncome.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly FixedIncomeDBContext _context;

        public ProductRepository(FixedIncomeDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FixedIncomeProduct>> GetAllOrderedByTaxDescAsync()
        {
            return await _context.Products
                .OrderByDescending(p => p.Tax)
                .ToListAsync();
        }

        public async Task<FixedIncomeProduct?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task UpdateAsync(FixedIncomeProduct product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
