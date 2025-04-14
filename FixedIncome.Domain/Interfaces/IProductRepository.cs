using FixedIncome.Domain.Entities;

namespace FixedIncome.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<FixedIncomeProduct>> GetAllOrderedByTaxDescAsync();
        Task<FixedIncomeProduct?> GetByIdAsync(int id);
        Task UpdateAsync(FixedIncomeProduct product);
    }
}
