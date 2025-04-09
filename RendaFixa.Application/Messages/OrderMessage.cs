namespace FixedIncome.Application.Messages;

public class OrderMessage
{
    public int AccountId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
