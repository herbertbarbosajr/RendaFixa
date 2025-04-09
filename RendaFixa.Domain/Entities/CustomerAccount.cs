namespace FixedIncome.Domain.Entities
{
    public class CustomerAccount(int account, string clientId, decimal balance)
    {
        public int Account { get; set; } = account;
        public string ClientId { get; set; } = clientId;
        public decimal Balance { get; set; } = balance;
    }
}
