namespace FixedIncome.Infrastructure.Events
{
    public class PurchaseRealizedEvent
    {
        public PurchaseRealizedEvent(int productId, int amount, decimal total, DateTime date)
        {
            ProductId = productId;
            Amount = amount;
            Total = total;
            Date = date;
        }
        public PurchaseRealizedEvent()
        {
        }

        public int ProductId { get; set; }
        public int Amount { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
    }
}
