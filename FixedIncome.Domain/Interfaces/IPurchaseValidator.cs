using FixedIncome.Domain.Entities;

namespace FixedIncome.Domain.Interfaces;

public interface IPurchaseValidator
{
    decimal ExecutePurchase(CustomerAccount account, FixedIncomeProduct product, int quantity);
    void OrderValidator(CustomerAccount account, FixedIncomeProduct product, int quantity);
}
