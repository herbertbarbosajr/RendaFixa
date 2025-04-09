using FixedIncome.Application.DTO_s;

namespace FixedIncome.Application.Interfaces
{
    public interface IOrderService
    {
        Task<bool> PurchaseAsync(PurchaseRequest request);
    }
}
