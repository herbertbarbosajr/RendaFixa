using FixedIncome.Application.DTO_s;
using FixedIncome.Domain.Entities;

namespace FixedIncome.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<FixedIncomeProduct>> GetOrderedProducts();
        Task MakePurchase(int productId, int amount);
        Task<decimal> GetBalance(int id);
    }
}
