namespace FixedIncome.Infrastructure.Events
{
    public record PurchaseRealizedEvent(int ProductId, int Amount, decimal Total, DateTime Date);

}
