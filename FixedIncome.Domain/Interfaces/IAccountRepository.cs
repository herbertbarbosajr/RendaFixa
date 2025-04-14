using FixedIncome.Domain.Entities;

namespace FixedIncome.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<CustomerAccount?> GetByIdAsync(int accountId);
        Task UpdateAsync(CustomerAccount account);
    }
}
