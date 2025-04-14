using FixedIncome.Domain.Entities;
using FixedIncome.Domain.Interfaces;

namespace FixedIncome.Domain.Validators
{
    public class PurchaseValidator : IPurchaseValidator
    {
        public void OrderValidator(CustomerAccount account, FixedIncomeProduct product, int quantity)
        {
            if (account == null)
                throw new InvalidOperationException("Conta não encontrada.");

            if (product == null)
                throw new InvalidOperationException("Produto não encontrado.");

            var totalPrice = product.UnitPrice * quantity;

            if (account.Balance < totalPrice)
                throw new InvalidOperationException("Saldo insuficiente.");

            if (product.Stock < quantity)
                throw new InvalidOperationException("Estoque insuficiente.");
        }

        public decimal ExecutePurchase(CustomerAccount account, FixedIncomeProduct product, int quantity)
        {
            if (product == null)
                throw new InvalidOperationException("Produto não encontrado.");

            if (account == null)
                throw new InvalidOperationException("Conta não encontrada.");

            var totalPrice = product.UnitPrice * quantity;
            account.Balance -= totalPrice;
            product.Stock -= quantity;
            return totalPrice;
        }
    }
}
