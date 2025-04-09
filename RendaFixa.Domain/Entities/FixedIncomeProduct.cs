namespace FixedIncome.Domain.Entities
{
    public class FixedIncomeProduct(string bondAsset, string index, double tax, string issuerName, decimal unitPrice, int stock)
    {
        public int Id { get; set; }
        public string BondAsset { get; set; } = bondAsset;
        public string Index { get; set; } = index;
        public double Tax { get; set; } = tax;
        public string IssuerName { get; set; } = issuerName;
        public decimal UnitPrice { get; set; } = unitPrice;
        public int Stock { get; set; } = stock;


        public void DebitStock(int amount)
        {
            if (amount > Stock)
                throw new InvalidOperationException("Insufficient Stock.");

            Stock -= amount;
        }
    }
}
