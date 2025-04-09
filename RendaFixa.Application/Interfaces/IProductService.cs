using FixedIncome.Application.DTO_s;

namespace FixedIncome.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetOrderedProducts();
        Task MakePurchase(int productId, int amount);
        Task<decimal> GetBalance(int id);
    }
}
